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

async function getPagination(page, condition) {
    return await httpService
        .get("/Employee/searchPagination/" + page + "?condition=" + condition)
        .then((res) => {
            return handleSuccess(res);
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
    getPagination,
};
