import { responsivePropType } from "react-bootstrap/esm/createUtilityClasses";
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
        .get("/Employee/SearchPagination/" + page + "?condition=" + condition)
        .then((res) => {
            return handleSuccess(res);
        })
        .catch((error) => {
            return handleError(error);
        });
}

async function SetImage(id, image) {
    return await httpService
        .put("/Employee/SetImage/" + id, image)
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
    GetPagination,
    SetImage,
};
