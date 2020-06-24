<template>
  <md-select
    v-model="selectedBudget"
    name="budget"
    id="budget"
    placeholder="Select budget..."
    @md-selected="onSelected"
    @md-opened="onOpened"
  >
    <md-option v-if="budgets.length === 0" disabled>Loading...</md-option>
    <md-option v-else v-for="item in budgets" :key="item.id" :value="item.id">{{ item.name }}</md-option>
  </md-select>
</template>

<script>
import Store from "src/services/store.service";
import Api from "src/services/api.service";

export default {
  name: "BudgetSelector",
  data() {
    return {
      selectedBudget: null
    };
  },
  computed: {
    budgets: () => Store.budgets
  },
  methods: {
    onOpened() {
      if (this.budgets.length === 0) {
        Api.getBudgets().then(budgets => {
          Store.budgets = budgets;
        });
      }
    },
    onSelected(item) {
      Store.clearCategories();
      Store.currentBudget = this.selectedBudget;
    }
  }
};
</script>
