import axios from "axios";
import { AxiosError } from "axios";
import { App } from "../constants";

export const httpService = axios.create({
    baseURL: App.URL + "/api/v1",
    headers: {
        "Content-Type": "application/json",
    },
});

export async function get(name) {
    return await httpService
        .get("/" + name)
        .then((res) => {
            return handleSuccess(res);
        })
        .catch((error) => {
            return handleError(error);
        });
}

export async function getById(name, id) {
    return await httpService
        .get("/" + name + "/" + id)
        .then((res) => {
            return handleSuccess(res);
        })
        .catch((error) => {
            return handleError(error);
        });
}
export async function add(name, entity) {
    return await httpService
        .post("/" + name, entity)
        .then((res) => {
            return handleSuccess(res);
        })
        .catch((error) => {
            return handleError(error);
        });
}
export async function edit(name, id, entity) {
    return await httpService
        .put("/" + name + "/" + id, entity)
        .then((res) => {
            return handleDeleteSuccess(res);
        })
        .catch((error) => {
            return handleError(error);
        });
}
export async function remove(name, id) {
    return await httpService
        .delete("/" + name + "/" + id)
        .then((res) => {
            return handleDeleteSuccess(res);
        })
        .catch((error) => {
            return handleError(error);
        });
}

export function handleSuccess(res) {
    if (App.DEV) console.table(res.data);
    return { ok: true, data: res.data };
}

export function handleDeleteSuccess(res) {
    if (App.DEV) console.table(res.data);
    return { ok: true, data: [createMessage("Message", res.data)] };
}

export function handleError(error) {
    if (!error.response) {
        return {
            ok: false,
            data: [createMessage("Network Problem", "No response from server")],
        };
    }

    if (error.code == AxiosError.ERR_NETWORK) {
        return {
            ok: false,
            data: [createMessage("Network Problem", "Try again later")],
        };
    }

    switch (error.response.status) {
        case 503:
            return { ok: false, data: [createMessage("Server Problem", error.response.data)] };
        case 400:
            if (typeof error.response.data.errors !== "undefined") {
                return handle400(error.response.data.errors);
            }
            return { ok: false, data: [createMessage("Data Problem", error.response.data)] };
    }

    return { ok: false, data: error };
}

function handle400(error) {
    let messages = [];
    for (const key in error) {
        messages.push(createMessage(key, error[key][0]));
    }
    return { ok: false, data: messages };
}

function createMessage(property, message) {
    return { property: property, message: message };
}

export function getMessagesAlert(data) {
    let messages = "";
    if (Array.isArray(data)) {
        for (const p of data) {
            messages += p.property + ": " + p.message + "\n"; // \n -> go to next row
        }
    } else {
        messages = data;
    }
    return messages;
}
