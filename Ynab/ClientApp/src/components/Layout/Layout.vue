<template>
  <div>
    <Navbar />
    <Sidenav />
    <Content />
  </div>
</template>

<script>
import { Navbar, Sidenav, Content } from "@/components";

export default {
  name: "Layout",
  components: {
    Navbar,
    Sidenav,
    Content
  }
};
</script>

<style src="./Layout.scss" lang="scss" />