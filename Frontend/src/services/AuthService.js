import { handleSuccess, httpService } from "./httpService";

export async function authorizationService(userData) {
    return await httpService .post("/Authorization/token", userData)
    .then((res) => {return handleSuccess(res);})
    .catch((error) => {return {error: true, data: [{property: 'Authorization', message: error.response.data}]}})
}