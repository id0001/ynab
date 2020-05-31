<template>
  <div v-if="disabled" class="app-category-selector disabled">Select category...</div>
  <a v-else href="#!" ref="dropdown" class="app-category-selector" :data-target="ids.categoryItems">
    {{ displayText }}
    <i
      v-if="dropdown && dropdown.isOpen"
      class="material-icons right"
    >arrow_drop_up</i>
    <i v-else class="material-icons right">arrow_drop_down</i>
    <ul :id="ids.categoryItems" class="dropdown-content">
      <li disabled v-if="categories.length <= 0">
        <span>Loading...</span>
      </li>
      <li v-for="item in categories" :key="item.id">
        <a href="#!" :class="{group: item.isGroup}" @click="selectCategory(item)">{{ item.name }}</a>
      </li>
    </ul>
  </a>
</template>

<script>
import { Uuid } from "@/mixins";
import M from "materialize-css";
import Auth from "@/services/auth.service";
import Api from "@/services/api.service";
import Store from "@/services/store.service";

export default {
  name: "CategorySelector",
  components: {},
  mixins: [Uuid],
  data() {
    return {
      auth: Auth,
      store: Store,
      categories: [],
      displayText: "Select category...",
      ids: {
        categoryItems: "data-target-" + this.$uuid
      },
      disabled: !Store.selectedBudget,
      dropdown: null
    };
  },
  mounted() {
    this.initDropdown();
  },
  watch: {
    "store.selectedBudget"(newValue) {
      this.disabled = !newValue;
      this.$nextTick(() => {
        if (!this.disabled) {
          this.initDropdown();
        }
      });
    }
  },
  methods: {
    initDropdown() {
      const el = this.$refs.dropdown;
      this.dropdown = M.Dropdown.init(el, {
        coverTrigger: false,
        closeOnClick: false,
        onOpenStart: () => {
          if (this.categories.length === 0) {
            this.loadCategories();
          }
        }
      });
      console.log(this.dropdown);
    },
    loadCategories() {
      Api.categories(Store.selectedBudget.id).then(res => {
        this.categories = mapCategories(res);
        this.updateDropdown();
      });
    },
    updateDropdown() {
      //   var inst = M.Dropdown.getInstance(this.$refs.dropdown);
      if (this.dropdown.isOpen) {
        this.$nextTick(() => {
          this.dropdown.recalculateDimensions();
        });
      }
    },
    selectCategory(category) {
      if (category.isGroup) return;

      this.dropdown.close();
      this.displayText = category.name;
      this.$emit("category-selected", category);
    },
    noClick(evt) {
      console.log("fuc");
      evt.preventDefault();
      evt.stopPropagation();
      return false;
    }
  }
};

function mapCategories(categories) {
  let result = [];
  categories.forEach(group => {
    if (!group.hidden) {
      result.push({
        id: group.id,
        name: group.name,
        isGroup: true
      });

      group.categories.forEach(cat => {
        if (!cat.hidden) {
          result.push({
            id: cat.id,
            name: cat.name,
            isGroup: false
          });
        }
      });
    }
  });

  return result;
}
</script>

<style lang="scss" src="./CategorySelector.scss" />