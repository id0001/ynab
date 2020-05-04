<template>
  <div id="app" class="grey lighten-4">
    <div class="teal lighten-3 topnav-main">
      <a href="#" class="logo-burger">
        <i class="material-icons">menu</i>
      </a>
      <a href="#" class="logo">YNAB</a>
    </div>
    <aside class="sidenav-main white">
      <category-list></category-list>
    </aside>
    <div class="main">
      <category-burndown></category-burndown>
    </div>
  </div>
</template>

<style lang="scss">
*,
** {
  box-sizing: border-box;
  margin: 0;
  padding: 0;
}

body {
  overflow: hidden;
}
</style>

<style lang="scss" scoped>
@import url("https://fonts.googleapis.com/css?family=Montserrat:400,700|Roboto:100,300,400");
$border-color: #c9c9c9;

#app {
  display: grid;
  grid-template-rows: 60px auto;
  width: 100vw;
  height: 100vh;
}

.sidenav-main {
  position: fixed;
  z-index: 999;
  top: 60px;
  height: 100%;
  width: 260px;
  border-right: 1px $border-color solid;
}

.topnav-main {
  border-bottom: 1px $border-color solid;

  .logo {
    width: 260px;
    height: 60px;
    line-height: 60px;
    display: block;
    text-align: center;
    font-family: "Roboto", sans-serif;
    font-weight: bold;
    font-size: 16pt;
    color: black;

    :visited {
      color: black;
    }
  }

  .logo-burger {
    display: none;
    position: fixed;
    width: 60px;
    height: 60px;
    line-height: 60px;
    font-size: 30px;
    text-align: center;
    color: black;
    font-size: 35px;
  }
}

.main {
  padding-left: 260px;
  display: flex;
  align-items: center;
  justify-content: center;
}

@media only screen and (max-width: 768px) {
  .sidenav-main {
    position: absolute;
    transform: translate(-80%, 0%);
    box-shadow: #000 3px 0px 15px -10px;
  }

  .topnav-main {
    .logo-burger {
      display: block;
    }
  }

  .main {
    padding-left: 0;
  }
}
</style>

<script>
import CategoryBurndown from "src/Components/CategoryBurndown.vue";
import CategoryList from "src/Components/CategoryList.vue";

import * as Api from "src/Services/YnabApi.js";

export default {
  name: "App",
  components: {
    "category-burndown": CategoryBurndown,
    "category-list": CategoryList
  },
  mounted() {},
  data() {
    return {};
  },
  methods: {}
};
</script>