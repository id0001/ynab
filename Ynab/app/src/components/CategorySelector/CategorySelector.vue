<template>
  <md-select
    v-model="selectedCategory"
    name="category"
    id="category"
    placeholder="Select category..."
    @md-selected="onSelected"
    @md-opened="onOpened"
    :disabled="selectedBudget === null"
  >
    <md-option v-if="categories.length === 0" disabled>Loading...</md-option>
    <md-optgroup v-else v-for="group in categories" :key="group.id" :label="group.name">
      <md-option v-for="cat in group.categories" :key="cat.id" :value="cat.id">{{ cat.name }}</md-option>
    </md-optgroup>
  </md-select>
</template>

<script>
import Store from "src/services/store.service";
import Api from "src/services/api.service";

export default {
  name: "BudgetSelector",
  data() {
    return {
      selectedCategory: null
    };
  },
  computed: {
    selectedBudget: () => Store.currentBudget,
    categories: () => {
      if (!Store.currentMonth) return [];

      Store.monthCategories = buildMonthCategories(
        Store.allCategories,
        Store.currentMonth.categories
      );

      if (!containsCategory(Store.monthCategories, Store.currentCategory)) {
        Store.currentCategory = null;
      }

      return Store.monthCategories;
    }
  },
  methods: {
    onOpened() {
      if (this.categories.length === 0) {
        Api.getCategories(this.selectedBudget).then(categories => {
          Store.allCategories = categories;
        });
      }
    },
    onSelected(item) {
      Store.currentCategory = item;
    }
  }
};

function buildMonthCategories(allCategories, availableCategories) {
  return allCategories
    .filter(g => availableCategories.hasOwnProperty(g.id))
    .map(g => {
      g.categories = g.categories
        .filter(c => availableCategories[g.id].some(o => o.id === c.id))
        .map(c => {
          const oc = availableCategories[g.id].find(o => o.id === c.id);

          return {
            id: c.id,
            name: c.name,
            budgeted: oc.budgeted
          };
        });
      return g;
    });
}

function containsCategory(categories, id) {
  return categories.flatMap(g => g.categories.map(c => c.id)).includes(id);
}
</script>
