(() => {
    const endpoint = "/api/Mascotas";
    const form = document.querySelector("#mascota-form");
    const tbody = document.querySelector("#mascotas-table-body");
    const message = document.querySelector("#mascotas-message");
    const tableMessage = document.querySelector("#mascotas-table-message");
    const title = document.querySelector("#mascota-form-title");
    const submit = document.querySelector("#mascota-submit");
    const cancel = document.querySelector("#mascota-cancel");
    const createOnlyFields = form.querySelectorAll("[data-create-only]");
    let editingId = null;
    let pets = [];

    function formData() {
        if (editingId !== null) {
            return {
                Edad: Number(form.elements.Edad.value),
                Peso: Number(form.elements.Peso.value)
            };
        }

        return {
            Nombre: form.elements.Nombre.value.trim(),
            Especie: form.elements.Especie.value.trim(),
            Raza: form.elements.Raza.value.trim(),
            Edad: Number(form.elements.Edad.value),
            Peso: Number(form.elements.Peso.value)
        };
    }

    function validate() {
        FormUtils.clearErrors(form);
        let valid = true;
        if (editingId === null) {
            valid = FormUtils.validateRequiredText(form, ["Nombre", "Especie", "Raza"]);
        }
        valid = FormUtils.validatePositiveNumber(form, "Edad", "La edad") && valid;
        valid = FormUtils.validatePositiveNumber(form, "Peso", "El peso") && valid;
        return valid;
    }

    function setEditMode(isEditing) {
        createOnlyFields.forEach(field => { field.hidden = isEditing; });
    }

    function render() {
        tbody.replaceChildren();
        if (!pets.length) {
            const row = document.createElement("tr");
            const cell = FormUtils.makeCell("Todavía no hay mascotas registradas.", "empty-cell");
            cell.colSpan = 6;
            row.append(cell);
            tbody.append(row);
            return;
        }

        pets.forEach(pet => {
            const row = document.createElement("tr");
            row.append(
                FormUtils.makeCell(pet.nombre),
                FormUtils.makeCell(pet.especie),
                FormUtils.makeCell(pet.raza),
                FormUtils.makeCell(pet.edad),
                FormUtils.makeCell(`${pet.peso} kg`)
            );
            const actions = document.createElement("td");
            actions.className = "actions-cell";
            const edit = document.createElement("button");
            edit.className = "button button-edit";
            edit.type = "button";
            edit.textContent = "Editar";
            edit.addEventListener("click", () => beginEdit(pet));
            const remove = document.createElement("button");
            remove.className = "button button-delete";
            remove.type = "button";
            remove.textContent = "Eliminar";
            remove.addEventListener("click", () => deletePet(pet));
            actions.append(edit, remove);
            row.append(actions);
            tbody.append(row);
        });
    }

    async function load() {
        FormUtils.showTableMessage(tableMessage, "Cargando mascotas...");
        try {
            pets = await AppApi.get(endpoint) || [];
            render();
            FormUtils.showTableMessage(tableMessage, "");
        } catch (error) {
            FormUtils.showTableMessage(tableMessage, error.message, true);
        }
    }

    function beginEdit(pet) {
        editingId = pet.id;
        form.elements.Nombre.value = pet.nombre ?? "";
        form.elements.Especie.value = pet.especie ?? "";
        form.elements.Raza.value = pet.raza ?? "";
        form.elements.Edad.value = pet.edad ?? "";
        form.elements.Peso.value = pet.peso ?? "";
        setEditMode(true);
        title.textContent = "Actualizar edad y peso";
        submit.textContent = "Actualizar datos";
        cancel.hidden = false;
        FormUtils.showMessage(message, "", "");
        FormUtils.clearErrors(form);
        form.scrollIntoView({ behavior: "smooth", block: "start" });
    }

    function resetForm() {
        editingId = null;
        form.reset();
        setEditMode(false);
        title.textContent = "Nueva mascota";
        submit.textContent = "Guardar mascota";
        cancel.hidden = true;
        FormUtils.clearErrors(form);
        FormUtils.showMessage(message, "", "");
    }

    async function deletePet(pet) {
        const confirmed = await FormUtils.confirmAction({
            title: "Eliminar mascota",
            message: `¿Querés eliminar a "${pet.nombre}"?`
        });
        if (!confirmed) return;

        try {
            await AppApi.delete(`${endpoint}/${pet.id}`);
            if (editingId === pet.id) resetForm();
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
                ? "Mascota creada correctamente."
                : "Mascota actualizada correctamente.";
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
    document.querySelector("#mascotas-refresh").addEventListener("click", load);
    window.MascotasView = { load };
})();
