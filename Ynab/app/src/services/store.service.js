import Vue from 'vue';

const state = Vue.observable({
	budgets            : [],
	categories         : [],
	filteredCategories : [],
	currentBudget      : null,
	currentCategory    : null,
	currentMonth       : null
});

const actions = {
	clearCategories : () => {
		state.currentCategory = null;
		state.categories = [];
	},
	getCategory     : (id) => {
		return state.categories.flatMap((e) => e.categories).find((e) => e.id === id);
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
	get filteredCategories() {
		return state.filteredCategories;
	},
	set filteredCategories(v) {
		state.filteredCategories = v;
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
	get currentMonth() {
		return state.currentMonth;
	},
	set currentMonth(v) {
		state.currentMonth = v;
	},
	...actions
};
