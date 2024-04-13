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

// async function AddProductOnWarehouse(transactionId, warehouseId, productId, quantity) {
//     const entity = {
//         transactionId: transactionId,
//         warehouseId: warehouseId,
//         productId: productId,
//         quantity: quantity,
//     };

//     return await httpService
//         .post(
//             "/" +
//                 entityName +
//                 "/Transactions/" +
//                 transactionId +
//                 "/Warehouses/" +
//                 warehouseId +
//                 "/Products/" +
//                 productId,
//             entity
//         )
//         .then((res) => {
//             if (App.DEV) console.table(res.data);

//             return res;
//         })
//         .catch((error) => {
//             console.log(error);
//         });
// }

export default {
    get,
    remove,
    add,
    edit,
    getById,
    GetProducts,
    GetWarehouses,
    GetProductsOnWarehouse,
    // AddProductOnWarehouse,
};
