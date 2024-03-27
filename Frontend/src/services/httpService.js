import axios from "axios";

export const httpService = axios.create({
    // baseURL: "https://ivorcelic-001-site1.htempurl.com/api/v1",
    baseURL: "https://localhost:7183/api/v1",
    headers: {
        "Content-Type": "application/json",
    },
});
