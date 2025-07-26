<script setup>
import LayoutDefault from '@/shared/components/layouts/LayoutDefault.vue';
// 导入我们真正需要的新组件：公告轮播
import AnnouncementCarousel from '@/shared/components/AnnouncementCarousel.vue';
</script>

<template>
  <main>
    <LayoutDefault>
      
      <!--
        我们将所有新内容都放在一个容器里，
        这个容器会被插入到 LayoutDefault 的 <slot /> 中
      -->
      <div class="page-content-wrapper">
        
        <!-- 1. 在所有子页面的顶部，统一显示公告轮播 -->
        <AnnouncementCarousel />
        
        <!-- 2. 子路由的出口，所有子页面都会在这里渲染 -->
        <router-view />
        
      </div>

    </LayoutDefault>
  </main>
</template>

<style scoped>
.page-content-wrapper {
  /* 为内容区域添加一些内边距，让它不至于贴着边 */
  padding: 20px;
}
</style>