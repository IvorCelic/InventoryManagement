import { App } from "../constants";
import { httpService } from "./httpService";

async function getLocations() {
    return await httpService.get('/Location')
        .then((res) => {
            if (App.DEV) console.table(res.data);

            return res;
        })
        .catch((error) => {
            console.log(error);
        });
}

async function deleteLocation(id) {
    return await httpService.delete('/Location/' + id)
        .then((res) => {
            return { ok: true, message: res };
        })
        .catch((error) => {
            console.log(error);
        });
}

async function addLocation(location) {
    const response = await httpService.post('/Location', location)
        .then(() => {
            return { ok: true, message: 'Location added successfully.' }
        })
        .catch((error) => {
            console.log(error.response.data.errors);
            return { ok: false, message: 'Error' }
        });

    return response;
}

async function editLocation(id, location) {
    const response = await httpService.put('/Location/' + id, location)
    .then(() => {
        return { ok: true, message: 'Location edited successfully.'}
    })
    .catch((error) => {
        console.log(error.response.data.errors);
        return { ok: false, message: 'Error'}
    });

    return response;
}

async function getById(id) {
    return await httpService.get('/Location/' + id)
    .then((res) => {
        if (App.DEV) console.table(res.data);

        return res;
    })
    .catch((error) => {
        console.log(error);
        return { message: error}
    })
}

export default {
    getLocations,
    deleteLocation,
    addLocation,
    editLocation,
    getById
}