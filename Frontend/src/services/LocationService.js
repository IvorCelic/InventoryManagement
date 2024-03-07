import { App } from "../constants";
import { httpService } from "./httpService";

async function getLocations() {
    return await httpService.get('/Location')
    .then((res) => {
        if (App.DEV) console.table(res.data);

        return res;
    })
    .catch((e) => {
        console.log(e);
    });
}

async function deleteLocation(id) {
    return await httpService.delete('/Location/' + id)
    .then((res) => {        
        return {ok: true, message: res};
    })
    .catch((e) => {
        console.log(e);
    });
}

export default {
    getLocations,
    deleteLocation
}