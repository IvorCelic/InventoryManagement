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

async function GetPagination(page, condition) {
    return await httpService
        .get("Product/SearchPagination/" + page + "?condition=" + condition)
        .then((res) => {
            return handleSuccess(res);
        })
        .catch((error) => {
            return handleError(error);
        });
}

async function GenerateQRCode(name, productId) {
    return await httpService
        .get("/" + name + `?productId=${productId}`, { responseType: "arraybuffer" })
        .then((response) => {
            return handleSuccess(response);
        })
        .catch((error) => {
            return handleError(error);
        });
}

async function GenerateAllQRs(name) {
    return await httpService
        .get("/" + name + "/GenerateAllQRs", { responseType: "arraybuffer" })
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
    GetPagination,
    GenerateQRCode,
    GenerateAllQRs,
};
