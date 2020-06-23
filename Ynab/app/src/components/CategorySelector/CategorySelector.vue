<template>
  <md-select
    v-model="selectedCategory"
    name="category"
    id="category"
    placeholder="Select budget..."
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
    categories: () => Store.categories
  },
  methods: {
    onOpened() {
      if (this.categories.length === 0) {
        Api.categories(this.selectedBudget).then(categories => {
          console.log(categories);
          Store.categories = mapCategories(categories);
        });
      }
    },
    onSelected(item) {
      Store.currentCategory = item;
    }
  }
};

function mapCategories(groups) {
  return groups
    .filter(g => !g.hidden)
    .map(g => {
      return {
        id: g.id,
        name: g.name,
        categories: g.categories
          .filter(c => !c.hidden)
          .map(c => {
            return {
              id: c.id,
              name: c.name
            };
          })
      };
    });
}
</script>
