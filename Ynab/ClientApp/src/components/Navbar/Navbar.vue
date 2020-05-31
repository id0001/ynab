<template>
  <nav class="app-navbar teal darken-2">
    <div class="navbar-wrapper">
      <a href="#!" class="brand-logo">{{ title }}</a>
      <a href="#" class="sidenav-trigger" data-target="app-sidenav">
        <i class="material-icons">menu</i>
      </a>
      <ul class="right hide-on-med-and-down">
        <li>
          <BudgetSelector />
        </li>
        <li>&gt;</li>
        <li>
          <CategorySelector />
        </li>
        <li>
          <AuthButton />
        </li>
      </ul>
    </div>
  </nav>
</template>

<script>
import { AuthButton, BudgetSelector, CategorySelector } from "@/components";

export default {
  name: "Navbar",
  components: { AuthButton, BudgetSelector, CategorySelector },
  data() {
    return {
      title: "YNAB"
    };
  }
};
</script>

<style lang="scss" src="./Navbar.scss" />