<template>
  <div class="root">
    <Navbar />
    <div class="wrapper">
      <Sidebar />
      <div class="content">Test</div>
    </div>
  </div>
</template>

<script>
import { Navbar, Sidebar } from "@/components";

export default {
  name: "Layout",
  components: { Navbar, Sidebar }
};
</script>

<style src="./Layout.scss" lang="scss" />