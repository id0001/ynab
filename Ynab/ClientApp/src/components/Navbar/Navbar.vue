<template>
  <nav>
    <div class="nav-wrapper">
      <a href="#" class="brand-logo">YNAB</a>
      <a href="#" data-target="app-sidenav" class="sidenav-trigger">
        <i class="material-icons">menu</i>
      </a>
      <ul class="right hide-on-med-and-down">
        <li>
          <BudgetSelector />
        </li>
        <li>
          <a href="/Login">Login</a>
        </li>
      </ul>
    </div>
  </nav>
</template>

<script>
import { BudgetSelector } from "@/components";

export default {
  name: "Navbar",
  components: { BudgetSelector }
};
</script>

<style lang="scss" src="./Navbar.scss" />