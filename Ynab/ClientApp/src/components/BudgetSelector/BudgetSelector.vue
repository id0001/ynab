<template>
  <a href="#!" ref="dropdown" class="app-budget-selector" :data-target="ids.budgetItems">
    {{ displayText }}
    <i v-if="dropdownOpen" class="material-icons right">arrow_drop_up</i>
    <i v-else class="material-icons right">arrow_drop_down</i>
    <ul :id="ids.budgetItems" class="dropdown-content">
      <li disabled v-if="budgets.length <= 0">
        <span>Loading...</span>
      </li>
      <li v-for="item in budgets" :key="item.id">
        <a href="#!" @click="selectBudget(item)">{{ item.name }}</a>
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
  name: "BudgetSelector",
  components: {},
  mixins: [Uuid],
  data() {
    return {
      auth: Auth,
      dropdownOpen: false,
      budgets: [],
      displayText: "Select budget...",
      ids: {
        budgetItems: "data-target-" + this.$uuid
      }
    };
  },
  mounted() {
    this.initDropdown();
  },
  methods: {
    initDropdown() {
      const el = this.$refs.dropdown;
      M.Dropdown.init(el, {
        coverTrigger: false,
        onOpenStart: () => {
          this.dropdownOpen = true;
          if (this.budgets.length === 0) {
            this.loadBudgets();
          }
        },
        onCloseStart: () => {
          this.dropdownOpen = false;
        }
      });
    },
    loadBudgets() {
      Api.budgets().then(res => {
        this.budgets = res;
        this.updateDropdown();
      });
    },
    updateDropdown() {
      var inst = M.Dropdown.getInstance(this.$refs.dropdown);
      if (inst.isOpen) {
        this.$nextTick(() => {
          inst.recalculateDimensions();
        });
      }
    },
    selectBudget(budget) {
      this.displayText = budget.name;
      this.$emit("budget-selected", budget);
      Store.selectedBudget = budget;
    }
  }
};
</script>

<style lang="scss" src="./BudgetSelector.scss" />