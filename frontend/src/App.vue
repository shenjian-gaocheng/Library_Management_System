<template>
  <router-view />

  <div id="app">
    <AppHeader />
    
    <main class="main-content">
      <!-- 路由视图容器 -->
      <router-view />
    </main>
    
    <AppFooter />
  </div>
</template>

<script setup>

</script>

<style>
  html, body, #app {
    margin: 0;
    padding: 0;
    width: 100%;
    height: 100%;
    overflow-x: hidden;
  }

  .main-content {
  min-height: calc(100vh - 120px); /* 根据实际页眉页脚高度调整 */
  padding: 2rem 0;
  }
</style>
