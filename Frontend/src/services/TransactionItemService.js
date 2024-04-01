import { App } from "../constants";
import { httpService } from "./httpService";

const entityName = "InventoryTransactionItem";

async function get() {
    return await httpService
        .get("/" + entityName)
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
            console.log("Added" + entityName);
            return { ok: true, message: entityName + " added successfully." };
        })
        .catch((error) => {
            console.log(error);
            return { ok: false, message: error.response.data };
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

async function GetProducts(id) {
    return await httpService
        .get("/" + entityName + "/Products/" + id)
        .then((res) => {
            if (App.DEV) console.table(res.data);

            return res;
        })
        .catch((error) => {
            console.log(error);
        });
}

async function GetWarehouses(id) {
    return await httpService
        .get("/" + entityName + "/Warehouses/" + id)
        .then((res) => {
            if (App.DEV) console.table(res.data);

            return res;
        })
        .catch((error) => {
            console.log(error);
        });
}

async function GetProductsOnWarehouse(transactionId, warehouseId) {
    return await httpService
        .get("/" + entityName + "/Transactions/" + transactionId + "/Warehouses/" + warehouseId)
        .then((res) => {
            if (App.DEV) console.table(res.data);

            return res;
        })
        .catch((error) => {
            console.log(error);
        });
}

async function AddProductOnWarehouse(transactionId, warehouseId, productId, quantity) {
    const entity = {
        transactionId: transactionId,
        warehouseId: warehouseId,
        productId: productId,
        quantity: quantity,
    };

    return await httpService
        .post(
            "/" +
                entityName +
                "/Transactions/" +
                transactionId +
                "/Warehouses/" +
                warehouseId +
                "/Products/" +
                productId,
            entity
        )
        .then((res) => {
            if (App.DEV) console.table(res.data);

            return res;
        })
        .catch((error) => {
            console.log(error);
        });
}

export default {
    get,
    remove,
    add,
    edit,
    getById,
    GetProducts,
    GetWarehouses,
    GetProductsOnWarehouse,
    AddProductOnWarehouse,
};
