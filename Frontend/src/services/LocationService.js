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

export default {
    getLocations
}