<template>
  <ul id="app-sidenav" class="app-sidenav sidenav">
    <li>
      <div class="user-view teal darken-2">
        <div class="background"></div>
      </div>
    </li>
    <li>
      <AuthButton />
    </li>
    <li v-if="auth.authenticated">
      <BudgetSelector />
    </li>
    <li v-if="auth.authenticated">
      <CategorySelector />
    </li>
    <li>
      <div class="divider"></div>
    </li>
    <li class="flex-fill">
      <div></div>
    </li>
  </ul>
</template>

<script>
import { AuthButton, BudgetSelector, CategorySelector } from "@/components";
import AuthService from "@/services/auth.service";

export default {
  name: "Sidenav",
  components: { AuthButton, BudgetSelector, CategorySelector },
  data() {
    return {
      auth: AuthService
    };
  },
  mounted() {
    const els = document.querySelectorAll(".sidenav");
    M.Sidenav.init(els);
  },
  computed: {
    authStatus() {
      if (this.auth.authenticated) {
        return "Logged in!";
      }

      return "Not logged in.";
    }
  }
};
</script>

<style lang="scss" src="./Sidenav.scss" />