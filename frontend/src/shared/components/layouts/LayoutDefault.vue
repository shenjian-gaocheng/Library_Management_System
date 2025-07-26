<template>
  <div>
    <!-- 1. 顶部导航条 (您项目原有的) -->
    <NavbarTop />
    
    <!-- 2. 插槽：这里会插入 HomeView.vue 中的 <router-view /> 所渲染出的内容 -->
    <slot />
    
  </div>
</template>

<script setup>
// 只保留最核心的 import
import NavbarTop from '@/shared/components/navigation/NavbarTop.vue'
</script>

<style scoped>
/* 您可以保留或清空这里的样式，暂时不影响功能 */
</style>