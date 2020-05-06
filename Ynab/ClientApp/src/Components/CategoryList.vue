<template>
  <div>
    <ul>
      <li v-for="cat in categories" :key="cat.id">{{ cat.name }}</li>
    </ul>
  </div>
</template>

<style lang="scss" scoped>
</style>

<script>
import * as Api from "src/Services/YnabApi";

export default {
  name: "category-list",
  mounted() {
    this.loadCategories();
  },
  data() {
    return {
      categories: []
    };
  },
  methods: {
    loadCategories() {
      Api.budgets()
        .then(budgets => {
          if (budgets.length > 0) {
            let b = budgets[0];
            return Api.categories(b.id);
          }
        })
        .then(res => {
          console.log(res);
          let cats = [];
          for (let group of res) {
            if (group.hidden || group.deleted) continue;

            cats.push({ id: group.id, name: group.name, isGroup: true });
            for (let category of group.categories) {
              if (category.hidden) continue;

              cats.push({
                id: category.id,
                name: category.name,
                isGroup: false
              });
            }
          }

          this.categories = cats;
        });
    }
  }
};
</script>