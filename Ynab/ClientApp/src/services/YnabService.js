var BASE_URL = 'http://localhost:5000';

const ensureAuthorizedResponse = (response) => {
	if (response.status == 401) {
		window.location = '/login';
	}
};

const isSuccessResponse = (response) => response.status >= 200 && response.status < 300;

const handleErrorResponse = (json, returnValue = null) => {
	//toastr.error(json.detail, json.name);
	return returnValue;
};

const handleResponse = (response, defaultReturnValue = null) => {
	ensureAuthorizedResponse(response);

	return isSuccessResponse(response)
		? response.json()
		: response.json().then((json) => handleErrorResponse(json), defaultReturnValue);
};

const get = (url) => fetch(url);

//=======================================
// Exports
//=======================================

export const budgets = () => get(BASE_URL + '/api/ynab/budgets').then((r) => handleResponse(r, []));

export const categories = (budget) =>
	get(BASE_URL + '/api/ynab/categories?budgetId=' + budget).then((r) => handleResponse(r, []));
