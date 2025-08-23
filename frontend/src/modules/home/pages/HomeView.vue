<template>
  <!-- 这里是您项目最外层的布局 -->
  <NavbarTop />

  <!-- 
    这是最关键的改动！
    所有子路由 (首页内容、公告管理、图书查询等) 都会被渲染到这里
  -->
  <main class="main-content">
    <router-view />
  </main>

  <Footer />
</template>

<script setup>
// 这个主布局文件，现在只负责引入布局组件
import NavbarTop from '@/shared/components/layouts/NavbarTop.vue'; // 您的 NavbarTop 路径可能不同
import Footer from '@/shared/components/layouts/Footer.vue';
</script>

<style scoped>
.main-content {
  /* 为主要内容区域添加一些样式，比如一个最小高度和内边距 */
  padding-top: 4rem; /* 预留出顶部导航栏的高度 */
  min-height: 100vh;
}
</style>