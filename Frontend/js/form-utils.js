window.FormUtils = {
    clearErrors(form) {
        form.querySelectorAll(".field-error").forEach(error => { error.textContent = ""; });
        form.querySelectorAll(".field.has-error").forEach(field => field.classList.remove("has-error"));
    },

    setError(form, name, message) {
        const error = form.querySelector(`[data-error-for="${name}"]`);
        if (error) {
            error.textContent = message;
            error.closest(".field")?.classList.add("has-error");
        }
    },

    validateRequiredText(form, names) {
        let valid = true;
        names.forEach(name => {
            const input = form.elements.namedItem(name);
            if (!input.value.trim()) {
                this.setError(form, name, "Este campo es obligatorio.");
                valid = false;
            }
        });
        return valid;
    },

    validatePositiveNumber(form, name, label) {
        const input = form.elements.namedItem(name);
        const value = Number(input.value);
        if (input.value.trim() === "" || !Number.isFinite(value) || value <= 0) {
            this.setError(form, name, `${label} debe ser un número mayor que 0.`);
            return false;
        }
        return true;
    },

    showMessage(element, message, type) {
        element.textContent = message;
        element.className = `form-message${type ? ` is-${type}` : ""}`;
    },

    showTableMessage(element, message, isError = false) {
        element.textContent = message;
        element.className = `table-message${isError ? " is-error" : ""}`;
    },

    makeCell(text, className) {
        const cell = document.createElement("td");
        cell.textContent = text ?? "";
        if (className) cell.className = className;
        return cell;
    },

    setBusy(button, busy, busyText) {
        if (busy) {
            button.dataset.originalText = button.textContent;
            button.textContent = busyText;
            button.disabled = true;
        } else {
            button.textContent = button.dataset.originalText || button.textContent;
            delete button.dataset.originalText;
            button.disabled = false;
        }
    },

    confirmAction({ title, message, confirmLabel = "Sí, eliminar" }) {
        const modal = document.querySelector("#confirm-modal");
        const card = modal.querySelector(".confirm-card");
        const titleElement = modal.querySelector("#confirm-title");
        const messageElement = modal.querySelector("#confirm-message");
        const cancel = modal.querySelector("#confirm-cancel");
        const accept = modal.querySelector("#confirm-accept");

        if (!modal.hidden) return Promise.resolve(false);

        titleElement.textContent = title;
        messageElement.textContent = message;
        accept.textContent = confirmLabel;
        modal.hidden = false;
        document.body.classList.add("has-open-modal");

        return new Promise(resolve => {
            const previousFocus = document.activeElement;
            const finish = confirmed => {
                modal.hidden = true;
                document.body.classList.remove("has-open-modal");
                cancel.removeEventListener("click", onCancel);
                accept.removeEventListener("click", onAccept);
                modal.removeEventListener("click", onBackdropClick);
                document.removeEventListener("keydown", onKeyDown);
                previousFocus?.focus();
                resolve(confirmed);
            };
            const onCancel = () => finish(false);
            const onAccept = () => finish(true);
            const onBackdropClick = event => {
                if (event.target === modal) finish(false);
            };
            const onKeyDown = event => {
                if (event.key === "Escape") {
                    finish(false);
                    return;
                }

                if (event.key === "Tab") {
                    const lastButton = accept;
                    if (event.shiftKey && document.activeElement === cancel) {
                        event.preventDefault();
                        lastButton.focus();
                    } else if (!event.shiftKey && document.activeElement === lastButton) {
                        event.preventDefault();
                        cancel.focus();
                    }
                }
            };

            cancel.addEventListener("click", onCancel);
            accept.addEventListener("click", onAccept);
            modal.addEventListener("click", onBackdropClick);
            document.addEventListener("keydown", onKeyDown);
            cancel.focus();
        });
    }
};
