const API_BASE_URL = "http://localhost:5151";

class ApiError extends Error {
    constructor(message, status) {
        super(message);
        this.name = "ApiError";
        this.status = status;
    }
}

async function request(path, method = "GET", data) {
    const options = {
        method,
        headers: { Accept: "application/json" }
    };

    if (data !== undefined) {
        options.headers["Content-Type"] = "application/json";
        options.body = JSON.stringify(data);
    }

    let response;
    try {
        response = await fetch(`${API_BASE_URL}${path}`, options);
    } catch (error) {
        throw new ApiError(
            "No se pudo conectar con la API. Verificá que esté iniciada y que la URL base sea correcta.",
            0
        );
    }

    const body = await response.text();
    let result = null;
    if (body) {
        try {
            result = JSON.parse(body);
        } catch {
            result = body;
        }
    }

    if (!response.ok) {
        throw new ApiError(getErrorMessage(result, response.status), response.status);
    }

    return result;
}

function getErrorMessage(body, status) {
    if (typeof body === "string" && body.trim()) return body;
    if (body && typeof body === "object") {
        if (typeof body.mensaje === "string") return body.mensaje;
        if (typeof body.message === "string") return body.message;
        if (typeof body.title === "string") return body.title;
        if (body.errors && typeof body.errors === "object") {
            return Object.values(body.errors).flat().join(" ");
        }
    }

    return `La API respondió con un error (HTTP ${status}).`;
}

window.AppApi = {
    get: path => request(path),
    post: (path, data) => request(path, "POST", data),
    put: (path, data) => request(path, "PUT", data),
    delete: path => request(path, "DELETE")
};
