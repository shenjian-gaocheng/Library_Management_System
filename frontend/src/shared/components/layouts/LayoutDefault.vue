<template>
  <div class="layout">
    <!-- 1. 顶部导航条 -->
    <NavbarTop />
    
    <!-- 2. 插槽：可插入任意组件 -->
    <div class="slot">
      <slot />
    </div>
    <!-- 3. 页脚信息栏 -->
    <Footer />
  </div>
</template>

<script setup>
import NavbarTop from '@/shared/components/layouts/NavbarTop.vue'
import Footer from '@/shared/components/layouts/Footer.vue'
</script>

<style scoped>
/* ① 让外层容器撑满视口并启用纵向 Flex */
.layout {
  display: flex;
  flex-direction: column;
  min-height: 100vh; 
}

/* ② 主内容自动拉伸，占据剩余空间 */
.slot {
  flex: 1 0 auto;      /* 吃掉剩余空间，可继续随内容增长 */
  padding-top: 64px;      /* 把内容整体下移，为顶部导航栏留出空间 */
}
</style>