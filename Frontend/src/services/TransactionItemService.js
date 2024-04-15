import {
    httpService,
    handleError,
    handleSuccess,
    get,
    remove,
    add,
    getById,
    edit,
} from "./httpService";

async function GetProducts(name, id) {
    return await httpService
        .get("/" + name + "/Products/" + id)
        .then((response) => {
            return handleSuccess(response);
        })
        .catch((error) => {
            return handleError(error);
        });
}

async function GetWarehouses(name, id) {
    return await httpService
        .get("/" + name + "/Warehouses/" + id)
        .then((response) => {
            return handleSuccess(response);
        })
        .catch((error) => {
            return handleError(error);
        });
}

async function GetProductsOnWarehouse(name, transactionId, warehouseId) {
    return await httpService
        .get("/" + name + "/Transactions/" + transactionId + "/Warehouses/" + warehouseId)
        .then((response) => {
            return handleSuccess(response);
        })
        .catch((error) => {
            return handleError(error);
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
};
