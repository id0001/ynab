export default {
	async getBudgets() {
		const response = await fetch('/api/ynab/budgets');
		return await response.json();
	},
	async getCategories(budgetId) {
		return await (await fetch(`/api/ynab/categories?budgetid=${encodeURIComponent(budgetId)}`)).json();
	},
	async getMonth(budgetId, month) {
		return await (await fetch(
			`/api/ynab/month?budgetid=${encodeURIComponent(budgetId)}&month=${encodeURIComponent(month.toISOString())}`
		)).json();
	},
	async getTransactions(budgetId, categoryId, month) {
		return await (await fetch(
			`/api/ynab/transactions?budgetid=${encodeURIComponent(budgetId)}&categoryid=${encodeURIComponent(
				categoryId
			)}&month=${encodeURIComponent(month.toISOString())}`
		)).json();
	}
};
