<template>
  <div class="app-root">
    <Navbar />
    <Sidenav />
    <div class="content"></div>
  </div>
</template>

<script>
import { Navbar, Sidenav } from "@/components";
import M from "materialize-css";

import Auth from "@/services/auth.service";

export default {
  name: "App",
  components: { Navbar, Sidenav },
  created() {
    Auth.init()
      .then(() => {
        if (!Auth.authenticated) {
          Auth.login();
        }
      })
      .catch(() => {});
  }
};
</script>

<style lang="scss" src="@/styles/app.scss" />