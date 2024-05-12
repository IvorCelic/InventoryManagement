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

async function GetUnassociatedProducts(name, id) {
    return await httpService
        .get("/" + name + "/UnassociatedProducts/" + id)
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

async function SearchUnassociatedProduct(name, transactionId, condition) {
    return await httpService
        .get("/" + name + "/SearchUnassociatedProduct/" + transactionId + "/" + condition)
        .then((response) => {
            return handleSuccess(response);
        })
        .catch((error) => {
            return handleError(error);
        });
}

async function SearchProductOnWarehouse(name, transactionId, warehouseId, condition) {
    return await httpService
        .get(
            "/" +
                name +
                "/SearchProductOnWarehouse/" +
                transactionId +
                "/" +
                warehouseId +
                "/" +
                condition
        )
        .then((response) => {
            return handleSuccess(response);
        })
        .catch((error) => {
            return handleError(error);
        });
}

async function GetUnassociatedProductsPagination(transactionId, page) {
    return await httpService
        .get(
            "/InventoryTransactionItem/UnassociatedProductsPagination/" + transactionId + "/" + page
        )
        .then((response) => {
            return handleSuccess(response);
        })
        .catch((error) => {
            return handleError(error);
        });
}

async function GetProductsOnWarehousePagination(transactionId, warehouseId, page) {
    return await httpService
        .get(
            "/InventoryTransactionItem/ProductsOnWarehousePagination/" +
                transactionId +
                "/" +
                warehouseId +
                "/" +
                page
        )
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
    GetUnassociatedProducts,
    GetWarehouses,
    GetProductsOnWarehouse,
    SearchUnassociatedProduct,
    SearchProductOnWarehouse,
    GetUnassociatedProductsPagination,
    GetProductsOnWarehousePagination,
};
