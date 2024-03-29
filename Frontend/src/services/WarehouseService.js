import { App } from "../constants";
import { httpService } from "./httpService";

const entityName = "Warehouse";

async function get() {
    return await httpService.get("/" + entityName)
        .then((res) => {
            if (App.DEV) console.table(res.data);

            return res;
        })
        .catch((error) => {
            console.log(error);
        });
}

async function remove(id) {
    return await httpService
        .delete("/" + entityName + "/" + id)
        .then((res) => {
            return { ok: true, message: res };
        })
        .catch((error) => {
            console.log(error);
        });
}

async function add(entity) {
    const response = await httpService
        .post("/" + entityName, entity)
        .then(() => {
            return { ok: true, message: entityName + " added successfully." };
        })
        .catch((error) => {
            console.log(error.response.data.errors);
            return { ok: false, message: "Error" };
        });

    return response;
}

async function edit(id, entity) {
    const response = await httpService
        .put("/" + entityName + "/" + id, entity)
        .then(() => {
            return { ok: true, message: entityName + " edited successfully." };
        })
        .catch((error) => {
            console.log(error.response.data.errors);
            return { ok: false, message: "Error" };
        });

    return response;
}

async function getById(id) {
    return await httpService
        .get("/" + entityName + "/" + id)
        .then((res) => {
            if (App.DEV) console.table(res.data);

            return res;
        })
        .catch((error) => {
            console.log(error);
            return { message: error };
        });
}

export default {
    get,
    remove,
    add,
    edit,
    getById,
};
