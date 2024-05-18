import {
    get,
    remove,
    add,
    edit,
    getById,
    httpService,
    handleSuccess,
    handleError,
} from "./httpService";

async function GetTransactionReport(name, transactionId) {
    return await httpService
        .get("/" + name + "/InventoryTransactionReport/?transactionId=" + transactionId, {
            responseType: "arraybuffer",
        })
        .then((response) => {
            console.log(response.data);
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
    GetTransactionReport,
};
