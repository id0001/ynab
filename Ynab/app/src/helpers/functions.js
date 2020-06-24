export function groupBy(array, selector) {
	return array.reduce(function(rv, x) {
		(rv[selector(x)] = rv[selector(x)] || []).push(x);
		return rv;
	}, {});
}
