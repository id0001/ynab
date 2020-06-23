import sleep from 'src/helpers/sleep';

export default {
	async budgets() {
		const response = await fetch('/api/ynab/budgets');
		return await response.json();
	},
	async categories(budgetId) {
		return await (await fetch(`/api/ynab/categories?budgetid=${encodeURIComponent(budgetId)}`)).json();
	}
};
