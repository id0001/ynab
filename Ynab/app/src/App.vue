<template>
  <md-app style="min-height: 100vh">
    <md-app-toolbar class="md-large md-primary">
      <div class="md-toolbar-row">
        <h3 class="md-title">YNAB</h3>
        <div class="md-toolbar-section-end">
          <md-button v-if="authenticated" @click="logout">Logout</md-button>
        </div>
      </div>
      <div class="md-toolbar-row center-content">
        <div class="md-layout md-gutter md-alignment-bottom-right">
          <div class="md-layout-item">
            <md-field>
              <label for="budget">Budget</label>
              <BudgetSelector />
            </md-field>
          </div>

          <div class="md-layout-item">
            <md-field>
              <label for="category">Category</label>
              <CategorySelector />
            </md-field>
          </div>
        </div>
      </div>
    </md-app-toolbar>

    <md-app-content>
      <Graph />
    </md-app-content>
  </md-app>
</template>

<script>
import { Graph, BudgetSelector, CategorySelector } from "src/components";
import Store from "src/services/store.service";
import Api from "src/services/api.service";
import Auth from "src/services/auth.service";

export default {
  name: "App",
  components: { Graph, BudgetSelector, CategorySelector },
  data() {
    return {};
  },
  computed: {
    authenticated: () => Auth.authenticated
  },
  mounted() {
    Auth.init().then(() => {
      if (!Auth.authenticated) {
        Auth.login();
      }
    });
  },
  methods: {
    logout: () => Auth.logout()
  }
};
</script>

<style lang="scss" src="./App.scss" />
