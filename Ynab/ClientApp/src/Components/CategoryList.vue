<template>
  <div>
    <ul>
      <li v-for="cat in categories" :key="cat">{{ cat }}</li>
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
      Api.getCategories().then(res => (this.categories = res));
    }
  }
};
</script>