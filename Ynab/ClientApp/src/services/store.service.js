import Vue from "vue";

const _store = Vue.observable({
  selectedBudget: null
});

export default {
  selectedBudget: _store.selectedBudget
};
