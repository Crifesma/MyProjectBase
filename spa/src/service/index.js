import conectApi from "./setup";

//class is defined with generally used methods (CRUD)
class service {
	setEndPoint(_url) {
		console.log(_url);
		this.url = _url;
	}

	get(id, parameters) {
		let source = `${this.url}/`;
		if (id !== undefined) source = source + `${id}`;
		console.log(parameters);
		let searchTerm = "";
		let searchProperty = "";
		let dateSearch = "";
		if (parameters.searchTerm != null)
			searchTerm = "&searchTerm=" + parameters.searchTerm;
		if (parameters.searchProperty != null)
			searchProperty = "&searchProperty=" + parameters.searchProperty;
		if (parameters.year != null)
			dateSearch = `&year=${parameters.year}&month=${parameters.month}&day=${parameters.day}`;

		return conectApi.get(
			source +
				"?currentPage=" +
				parameters.currentPage +
				"&pageSize=" +
				parameters.pageSize +
				searchTerm +
				searchProperty +
				dateSearch
		);
	}

	getAll(parameters) {
		let source = `${this.url}/`;
		console.log(parameters);
		let searchTerm = "";
		let searchProperty = "";
		let dateSearch = "";
		if (parameters.searchTerm != null)
			searchTerm = "&searchTerm=" + parameters.searchTerm;
		if (parameters.searchProperty != null)
			searchProperty = "&searchProperty=" + parameters.searchProperty;
		if (parameters.year != null)
			dateSearch = `&year=${parameters.year}&month=${parameters.month}&day=${parameters.day}`;

		return conectApi.get(
			source +
				"?currentPage=" +
				parameters.currentPage +
				"&pageSize=" +
				parameters.pageSize +
				searchTerm +
				searchProperty +
				dateSearch
		);
	}

	//get with AND , the other is with OR
	filter(filterDatas, parameters) {
		return conectApi.post(
			`${this.url}/Filter?currentPage=${parameters.currentPage}&pageSize=${parameters.pageSize}`,
			filterDatas
		);
	}

	post(data) {
		return conectApi.post(`${this.url}`, data);
	}
	put(id, data) {
		return conectApi.put(`${this.url}/${id}`, data);
	}

	delete(id) {
		return conectApi.delete(`${this.url}/${id}`);
	}
}

export default service;
