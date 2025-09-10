<template>
  <!-- 首页轮播图和图书搜索栏 -->
  <Banner />

  <!-- 快捷入口模块 -->
  <QuickEntry />

  <!-- 推荐图书模块 -->
  <BookRecommend />

  <!-- 公告栏模块 -->
  <Announcement />

  <!-- 移除了这里的 <Footer /> -->
</template>

<script setup>
// 只导入本页面需要的核心内容组件
import Banner from '@/modules/home/components/Banner.vue'
import QuickEntry from '@/modules/home/components/QuickEntry.vue'
import BookRecommend from '@/modules/home/components/BookRecommend.vue'
import Announcement from '@/modules/home/components/Announcement.vue'

// 移除了对 NavbarTop 和 Footer 的导入
</script>

<style scoped>
/* 样式保持不变 */
</style>