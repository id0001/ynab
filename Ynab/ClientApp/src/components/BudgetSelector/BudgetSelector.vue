<template>
  <a
    :disabled="loading"
    ref="dropdown"
    class="app-budget-selector waves-effect waves-light"
    href="#!"
    :data-target="dataTargetId"
  >
    <i v-if="isDropDownClosed" class="material-icons">keyboard_arrow_down</i>
    <i v-else class="material-icons">keyboard_arrow_up</i>
    <span>{{ displayText }}</span>
    <ul :id="dataTargetId" class="dropdown-content">
      <li v-for="item in budgets" :key="item.id">
        <a @click="selectBudget(item)" href="#!">{{ item.name }}</a>
      </li>
    </ul>
  </a>
</template>
    
<script>
import * as Api from "@/services/YnabService.js";
import M from "materialize-css";
import Uuid from "@/mixins/Uuid";

export default {
  name: "BudgetSelector",
  mixins: [Uuid],
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
      loading: true,
      isDropDownClosed: true,
      displayText: this.placeholderText,
      budgets: [],
      dataTargetId: "data-target-" + this.$uuid
    };
  },
  mounted() {
    this.initializeDropdown();
    this.loadBudgets();
  },
  methods: {
    initializeDropdown() {
      M.Dropdown.init(this.$refs.dropdown, {
        coverTrigger: false,
        onOpenStart: () => {
          this.isDropDownClosed = false;
        },
        onCloseStart: () => {
          this.isDropDownClosed = true;
        }
      });
    },
    loadBudgets() {
      Api.budgets().then(items => {
        this.budgets = items;
        this.loading = false;
        console.log(items);
      });
    },
    selectBudget(budget) {
      this.displayText = budget.name;
      this.$emit("budget-selected", budget);
    }
  }
};
</script>

<style lang="scss" src="./BudgetSelector.scss" />