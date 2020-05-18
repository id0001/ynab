<template>
  <ul id="app-sidenav" class="sidenav sidenav-fixed">
    <li>
      <BudgetSelector />
    </li>
    <li>
      <a href="/Login">Login</a>
    </li>
  </ul>
</template>

<script>
import { BudgetSelector } from "@/components";
import M from "materialize-css";

export default {
  name: "Sidenav",
  components: { BudgetSelector },
  mounted() {
    var elem = document.querySelector("#app-sidenav");
    M.Sidenav.init(elem, {});
  }
};
</script>