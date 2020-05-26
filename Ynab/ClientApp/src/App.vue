<template>
  <div class="app-root">
    <Navbar />
    <Sidenav />
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
    console.log(Auth);
    Auth.init().catch(() => {});
  }
};
</script>

<style lang="scss" src="@/styles/app.scss" />