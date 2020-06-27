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
      if (!Store.currentMonth) return Store.categories;

      var filtered = filterCategories(
        Store.categories,
        Store.currentMonth.categories
      );
      if (!containsCategory(filtered, Store.currentCategory)) {
        Store.currentCategory = null;
      }

      return filtered;
    }
  },
  methods: {
    onOpened() {
      if (this.categories.length === 0) {
        Api.getCategories(this.selectedBudget).then(categories => {
          Store.categories = categories;
        });
      }
    },
    onSelected(item) {
      Store.currentCategory = item;
    }
  }
};

function filterCategories(allCategories, currentCategories) {
  return allCategories
    .filter(g => currentCategories.hasOwnProperty(g.id))
    .map(g => {
      g.categories = g.categories.filter(c =>
        currentCategories[g.id].includes(c.id)
      );
      return g;
    });
}

function containsCategory(categories, id) {
  return categories.flatMap(g => g.categories.map(c => c.id)).includes(id);
}
</script>
