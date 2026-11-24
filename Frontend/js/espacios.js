(() => {
    const endpoint = "/Espacio";
    const form = document.querySelector("#espacio-form");
    const tbody = document.querySelector("#espacios-table-body");
    const message = document.querySelector("#espacios-message");
    const tableMessage = document.querySelector("#espacios-table-message");
    const title = document.querySelector("#espacio-form-title");
    const submit = document.querySelector("#espacio-submit");
    const cancel = document.querySelector("#espacio-cancel");
    let editingId = null;
    let spaces = [];

    function formData() {
        return {
            Nombre: form.elements.Nombre.value.trim(),
            Tamano: Number(form.elements.Tamano.value),
            Capacidad: Number(form.elements.Capacidad.value),
            Estado: form.elements.Estado.value === "true",
            Tipo: form.elements.Tipo.value.trim()
        };
    }

    function validate() {
        FormUtils.clearErrors(form);
        let valid = FormUtils.validateRequiredText(form, ["Nombre", "Tipo"]);
        valid = FormUtils.validatePositiveNumber(form, "Tamano", "El tamaño") && valid;
        valid = FormUtils.validatePositiveNumber(form, "Capacidad", "La capacidad") && valid;
        return valid;
    }

    function render() {
        tbody.replaceChildren();
        if (!spaces.length) {
            const row = document.createElement("tr");
            const cell = FormUtils.makeCell("Todavía no hay espacios registrados.", "empty-cell");
            cell.colSpan = 6;
            row.append(cell);
            tbody.append(row);
            return;
        }

        spaces.forEach(space => {
            const row = document.createElement("tr");
            row.append(
                FormUtils.makeCell(space.nombre),
                FormUtils.makeCell(space.tipo),
                FormUtils.makeCell(space.tamano),
                FormUtils.makeCell(space.capacidad)
            );
            const statusCell = document.createElement("td");
            const pill = document.createElement("span");
            pill.className = `status-pill ${space.estado ? "is-available" : "is-unavailable"}`;
            pill.textContent = space.estado ? "Disponible" : "No disponible";
            statusCell.append(pill);
            row.append(statusCell);

            const actions = document.createElement("td");
            actions.className = "actions-cell";
            const edit = document.createElement("button");
            edit.className = "button button-edit";
            edit.type = "button";
            edit.textContent = "Editar";
            edit.addEventListener("click", () => beginEdit(space));
            const remove = document.createElement("button");
            remove.className = "button button-delete";
            remove.type = "button";
            remove.textContent = "Eliminar";
            remove.addEventListener("click", () => deleteSpace(space));
            actions.append(edit, remove);
            row.append(actions);
            tbody.append(row);
        });
    }

    async function load() {
        FormUtils.showTableMessage(tableMessage, "Cargando espacios...");
        try {
            spaces = await AppApi.get(endpoint) || [];
            render();
            FormUtils.showTableMessage(tableMessage, "");
        } catch (error) {
            FormUtils.showTableMessage(tableMessage, error.message, true);
        }
    }

    function beginEdit(space) {
        editingId = space.id;
        form.elements.Nombre.value = space.nombre ?? "";
        form.elements.Tamano.value = space.tamano ?? "";
        form.elements.Capacidad.value = space.capacidad ?? "";
        form.elements.Estado.value = String(Boolean(space.estado));
        form.elements.Tipo.value = space.tipo ?? "";
        title.textContent = "Editar espacio";
        submit.textContent = "Guardar cambios";
        cancel.hidden = false;
        FormUtils.showMessage(message, "", "");
        FormUtils.clearErrors(form);
        form.scrollIntoView({ behavior: "smooth", block: "start" });
    }

    function resetForm() {
        editingId = null;
        form.reset();
        form.elements.Estado.value = "true";
        title.textContent = "Nuevo espacio";
        submit.textContent = "Guardar espacio";
        cancel.hidden = true;
        FormUtils.clearErrors(form);
        FormUtils.showMessage(message, "", "");
    }

    async function deleteSpace(space) {
        const confirmed = await FormUtils.confirmAction({
            title: "Eliminar espacio",
            message: `¿Querés eliminar el espacio "${space.nombre}"?`
        });
        if (!confirmed) return;

        try {
            await AppApi.delete(`${endpoint}/${space.id}`);
            if (editingId === space.id) resetForm();
            await load();
        } catch (error) {
            FormUtils.showTableMessage(tableMessage, error.message, true);
        }
    }

    form.addEventListener("submit", async event => {
        event.preventDefault();
        if (!validate()) return;

        FormUtils.setBusy(submit, true, "Guardando...");
        try {
            const data = formData();
            if (editingId === null) {
                await AppApi.post(endpoint, data);
            } else {
                await AppApi.put(`${endpoint}/${editingId}`, data);
            }
            const successMessage = editingId === null
                ? "Espacio creado correctamente."
                : "Espacio actualizado correctamente.";
            resetForm();
            await load();
            FormUtils.showMessage(message, successMessage, "success");
        } catch (error) {
            FormUtils.showMessage(message, error.message, "error");
        } finally {
            FormUtils.setBusy(submit, false);
        }
    });

    cancel.addEventListener("click", resetForm);
    document.querySelector("#espacios-refresh").addEventListener("click", load);
    window.EspaciosView = { load };
})();
