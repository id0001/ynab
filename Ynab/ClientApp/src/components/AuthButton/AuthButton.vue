<template>
  <a v-if="!auth.authenticated" href="/Login">Login</a>
  <a v-else href="/Logout">Logout</a>
</template>

<script>
import Auth from "@/services/auth.service";

export default {
  name: "AuthButton",
  components: {},
  data() {
    return {
      auth: Auth
    };
  },
  mounted() {},
  methods: {
    login() {
      Auth.login();
    }
  },
  computed: {}
};
</script>

<style lang="scss" src="./AuthButton.scss" />