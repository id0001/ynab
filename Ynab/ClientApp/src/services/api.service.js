import CachedFunction from "@/utilities/caching/CachedFunction";

const getCategory = new CachedFunction(budgetId => {
  return fetch("api/ynab/categories?budgetid=" + encodeURIComponent(budgetId)).then(response => {
    return response.json();
  });
});

const getBudgets = new CachedFunction(() => {
  return fetch("/api/ynab/budgets").then(response => {
    return response.json();
  });
});

export default {
  budgets() {
    return getBudgets.invoke();
  },
  categories(currentBudget) {
    return getCategory.invoke(currentBudget);
  }
};
