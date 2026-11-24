(() => {
    const endpoint = "/api/Estadias";
    const form = document.querySelector("#estadia-form");
    const message = document.querySelector("#estadias-message");
    const tableMessage = document.querySelector("#estadias-table-message");
    const tbody = document.querySelector("#estadias-table-body");
    const petSelect = form.elements.MascotaId;
    const spaceSelect = form.elements.EspacioId;
    const fechaIngreso = form.elements.FechaIngreso;
    const fechaSalida = form.elements.FechaSalida;
    const submit = form.querySelector('[type="submit"]');
    let petsById = new Map();
    let spacesById = new Map();
    let stays = [];

    function todayLocal() {
        const today = new Date();
        const year = today.getFullYear();
        const month = String(today.getMonth() + 1).padStart(2, "0");
        const day = String(today.getDate()).padStart(2, "0");
        return `${year}-${month}-${day}`;
    }

    function updateDateLimits() {
        const today = todayLocal();
        fechaIngreso.min = today;
        fechaSalida.min = fechaIngreso.value || today;
    }

    function validate() {
        FormUtils.clearErrors(form);
        let valid = true;

        if (!petSelect.value) {
            FormUtils.setError(form, "MascotaId", "Elegí una mascota.");
            valid = false;
        } else if (stays.some(stay => stay.mascotaId === Number(petSelect.value))) {
            FormUtils.setError(
                form,
                "MascotaId",
                "La mascota ya tiene una estadía registrada. Finalizá esa estadía antes de crear otra."
            );
            valid = false;
        }
        if (!spaceSelect.value) {
            const hayEspaciosDisponibles = [...spacesById.values()].some(space => space.estado);
            FormUtils.setError(
                form,
                "EspacioId",
                hayEspaciosDisponibles ? "Elegí un espacio." : "No hay espacios disponibles."
            );
            valid = false;
        } else if (spacesById.get(Number(spaceSelect.value))?.estado !== true) {
            FormUtils.setError(form, "EspacioId", "El espacio seleccionado no está disponible.");
            valid = false;
        }
        if (!fechaIngreso.value) {
            FormUtils.setError(form, "FechaIngreso", "La fecha de ingreso es obligatoria.");
            valid = false;
        } else if (fechaIngreso.value < todayLocal()) {
            FormUtils.setError(form, "FechaIngreso", "La fecha de ingreso no puede ser anterior a hoy.");
            valid = false;
        }
        if (!fechaSalida.value) {
            FormUtils.setError(form, "FechaSalida", "La fecha de salida es obligatoria.");
            valid = false;
        } else if (fechaIngreso.value && fechaSalida.value < fechaIngreso.value) {
            FormUtils.setError(form, "FechaSalida", "La salida no puede ser anterior al ingreso.");
            valid = false;
        }

        updateDateLimits();
        return valid;
    }

    function updateSelect(select, values, placeholder, labelForValue, disableOption = () => false) {
        const previousValue = select.value;
        select.replaceChildren(new Option(placeholder, ""));
        values.forEach(item => {
            const option = new Option(labelForValue(item), item.id);
            option.disabled = disableOption(item);
            select.add(option);
        });
        if (values.some(item => String(item.id) === previousValue)) {
            select.value = previousValue;
        }
        select.disabled = values.length === 0;
    }

    function render(stays) {
        tbody.replaceChildren();
        if (!stays.length) {
            const row = document.createElement("tr");
            const cell = FormUtils.makeCell("Todavía no hay estadías registradas.", "empty-cell");
            cell.colSpan = 5;
            row.append(cell);
            tbody.append(row);
            return;
        }

        stays.forEach(stay => {
            const pet = petsById.get(stay.mascotaId);
            const space = spacesById.get(stay.espacioId);
            const row = document.createElement("tr");
            row.append(
                FormUtils.makeCell(pet?.nombre || `Mascota #${stay.mascotaId}`),
                FormUtils.makeCell(space?.nombre || `Espacio #${stay.espacioId}`),
                FormUtils.makeCell(formatDate(stay.fechaIngreso)),
                FormUtils.makeCell(formatDate(stay.fechaSalida))
            );
            const actions = document.createElement("td");
            actions.className = "actions-cell";
            const finish = document.createElement("button");
            finish.className = "button button-delete";
            finish.type = "button";
            finish.textContent = "Finalizar";
            finish.setAttribute("aria-label", `Finalizar estadía de ${pet?.nombre || `mascota ${stay.mascotaId}`}`);
            finish.addEventListener("click", () => finalizarEstadia(stay));
            actions.append(finish);
            row.append(actions);
            tbody.append(row);
        });
    }

    function formatDate(value) {
        if (!value) return "";
        const [date] = value.split("T");
        const [year, month, day] = date.split("-");
        return year && month && day ? `${day}/${month}/${year}` : value;
    }

    async function finalizarEstadia(stay) {
        const petName = petsById.get(stay.mascotaId)?.nombre || `la mascota #${stay.mascotaId}`;
        const confirmed = await FormUtils.confirmAction({
            title: "Finalizar estadía",
            message: `¿Querés finalizar la estadía de ${petName}? El registro se eliminará y la mascota podrá tener una nueva estadía.`,
            confirmLabel: "Finalizar estadía"
        });
        if (!confirmed) return;

        try {
            await AppApi.delete(`${endpoint}/${stay.id}`);
            FormUtils.showMessage(message, `La estadía de ${petName} fue finalizada.`, "success");
            await load();
        } catch (error) {
            FormUtils.showMessage(message, error.message, "error");
        }
    }

    async function load() {
        FormUtils.showTableMessage(tableMessage, "Cargando estadías...");
        try {
            const [pets, spaces, loadedStays] = await Promise.all([
                AppApi.get("/api/Mascotas"),
                AppApi.get("/Espacio"),
                AppApi.get(endpoint)
            ]);

            petsById = new Map(pets.map(pet => [pet.id, pet]));
            spacesById = new Map(spaces.map(space => [space.id, space]));
            stays = loadedStays;
            updateSelect(petSelect, pets, "Elegí una mascota", pet => pet.nombre);
            updateSelect(
                spaceSelect,
                spaces,
                "Elegí un espacio",
                space => `${space.nombre} · ${space.tipo}${space.estado ? "" : " · No disponible"}`,
                space => !space.estado
            );
            updateDateLimits();
            render(stays);
            FormUtils.showTableMessage(tableMessage, "");
        } catch (error) {
            FormUtils.showTableMessage(tableMessage, error.message, true);
            FormUtils.showMessage(message, error.message, "error");
        }
    }

    form.addEventListener("submit", async event => {
        event.preventDefault();
        if (!validate()) return;

        FormUtils.setBusy(submit, true, "Registrando...");
        try {
            await AppApi.post(endpoint, {
                MascotaId: Number(petSelect.value),
                EspacioId: Number(spaceSelect.value),
                FechaIngreso: form.elements.FechaIngreso.value,
                FechaSalida: form.elements.FechaSalida.value
            });
            form.reset();
            FormUtils.clearErrors(form);
            FormUtils.showMessage(message, "Estadía registrada correctamente.", "success");
            await load();
        } catch (error) {
            FormUtils.showMessage(message, error.message, "error");
        } finally {
            FormUtils.setBusy(submit, false);
        }
    });

    fechaIngreso.addEventListener("change", updateDateLimits);
    document.querySelector("#estadias-refresh").addEventListener("click", load);
    window.EstadiasView = { load };
})();
