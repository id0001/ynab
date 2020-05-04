export function getCategories() {
	return get('/api/ynab/categories');
}

function get(url) {
	return fetch(url).then((response) => {
		if (response.status != 200) window.location = '/login';
		return response.json();
	});
}
