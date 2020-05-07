<template>
  <div class="app-budgetselector">
    <button
      :disabled="isLoading"
      ref="dropdownButton"
      class="waves-effect waves-light"
      data-target="budgets"
    >
      <i v-if="stateClosed" class="material-icons">keyboard_arrow_down</i>
      <i v-else class="material-icons">keyboard_arrow_up</i>
      <span>{{ buttonContent }}</span>
    </button>
    <ul id="budgets" class="dropdown-content">
      <li v-for="item in items" :key="item.id">
        <a @click="selectBudget(item)" href="#!">{{ item.name }}</a>
      </li>
    </ul>
  </div>
</template>

<script>
import M from "materialize-css";
import * as Api from "@/services/YnabService.js";

export default {
  name: "BudgetSelector",
  props: {
    placeholderText: {
      type: String,
      default() {
        return "Select budget...";
      }
    }
  },
  data() {
    return {
      isLoading: true,
      stateClosed: true,
      items: [],
      buttonContent: this.placeholderText
    };
  },
  mounted() {
    this.initDropdown();
    this.loadItems();
  },
  methods: {
    loadItems() {
      Api.budgets().then(items => {
        this.items = [
          { id: 1, name: "awooww wofwoe few ewof we wewefwef" },
          { id: 2, name: "test long 2" },
          { id: 2, name: "a" }
        ];
        this.isLoading = false;
      });
    },
    initDropdown() {
      let self = this;
      M.Dropdown.init(this.$refs.dropdownButton, {
        coverTrigger: false,
        onOpenStart: () => {
          this.stateClosed = false;
        },
        onCloseStart: () => {
          this.stateClosed = true;
        }
      });
    },
    selectBudget(budget) {
      this.buttonContent = budget.name;
      this.$emit("budget-selected", budget);
    }
  }
};
</script>

<style lang="scss" src="./BudgetSelector.scss" />