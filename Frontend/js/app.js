(() => {
    const views = {
        espacios: document.querySelector("#view-espacios"),
        mascotas: document.querySelector("#view-mascotas"),
        estadias: document.querySelector("#view-estadias")
    };
    const loaders = {
        espacios: window.EspaciosView.load,
        mascotas: window.MascotasView.load,
        estadias: window.EstadiasView.load
    };

    document.querySelectorAll(".nav-link").forEach(button => {
        button.addEventListener("click", () => {
            const selected = button.dataset.view;
            Object.entries(views).forEach(([name, view]) => {
                const isSelected = name === selected;
                view.hidden = !isSelected;
                view.classList.toggle("is-visible", isSelected);
            });
            document.querySelectorAll(".nav-link").forEach(link => {
                const isSelected = link === button;
                link.classList.toggle("is-active", isSelected);
                link.setAttribute("aria-current", isSelected ? "page" : "false");
            });
            loaders[selected]();
        });
    });

    loaders.espacios();
})();
