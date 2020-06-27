import Vue from "vue";

const state = Vue.observable({
  budgets: [],
  allCategories: [],
  monthCategories: [],
  currentBudget: null,
  currentCategory: null,
  currentMonth: null
});

const actions = {
  clearCategories: () => {
    state.currentCategory = null;
    state.categories = [];
  },
  getCategory: id => {
    return state.monthCategories.flatMap(e => e.categories).find(e => e.id === id);
  }
};

export default {
  get budgets() {
    return state.budgets;
  },
  set budgets(v) {
    state.budgets = v;
  },
  get allCategories() {
    return state.allCategories;
  },
  set allCategories(v) {
    state.allCategories = v;
  },
  get monthCategories() {
    return state.monthCategories;
  },
  set monthCategories(v) {
    state.monthCategories = v;
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
