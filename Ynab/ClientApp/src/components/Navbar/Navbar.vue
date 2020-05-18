<template>
  <div class="app-navbar">
    <a href="#" class="logo">YNAB</a>
    <ul class="left">
      <li>
        <BudgetSelector />
      </li>
    </ul>
    <ul class="right">
      <li>
        <a class="active waves-effect waves-light" href="#">Login</a>
      </li>
      <li>
        <a class="waves-effect waves-light" href="#">Test</a>
      </li>
      <li>
        <a class="waves-effect waves-light" href="#">Test32</a>
      </li>
      <li>
        <a class="waves-effect waves-light" href="#">Test3</a>
      </li>
    </ul>
  </div>
</template>

<script>
import M from "materialize-css";
// import { BudgetSelector } from "@/components";
import BudgetSelector from "@/components/BudgetSelector/BudgetSelector.vue";

export default {
  name: "Navbar",
  components: { BudgetSelector }
};
</script>

<style lang="scss" src="./Navbar.scss"/>
