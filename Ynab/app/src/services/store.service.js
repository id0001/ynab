import Vue from 'vue';

const state = Vue.observable({
	budgets         : [],
	categories      : [],
	currentBudget   : null,
	currentCategory : null
});

const actions = {
	clearCategories : () => {
		state.currentCategory = null;
		state.categories = [];
	}
};

export default {
	get budgets() {
		return state.budgets;
	},
	set budgets(v) {
		state.budgets = v;
	},
	get categories() {
		return state.categories;
	},
	set categories(v) {
		state.categories = v;
	},
	get currentBudget() {
		return state.currentBudget;
	},
	set currentBudget(v) {
		state.currentBudget = v;
	},
	get currentCategory() {
		return state.currentCategory;
	},
	set currentCategory(v) {
		state.currentCategory = v;
	},
	...actions
};
