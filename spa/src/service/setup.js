import axios from "axios";

// load config url API base using custom variable enviroment.

let configAxios = {
	baseURL: process.env.REACT_APP_BASE_URL_API,
};

const conectApi = axios.create(configAxios);

conectApi.interceptors.request.use((config) => {
	return config;
});

conectApi.interceptors.request.use((response) => {
	return response;
});

conectApi.interceptors.response.use(
	(response) => response,
	(error) => {
		return Promise.reject(error);
	}
);

export default conectApi;
