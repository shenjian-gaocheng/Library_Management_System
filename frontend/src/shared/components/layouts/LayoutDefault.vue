<template>
  <div>
    <!-- 1. 顶部导航条 -->
    <NavbarTop />

    <!-- 2. 插槽：可插入任意组件 -->
    <slot />


  </div>
</template>

<script setup>
import NavbarTop from '@/shared/components/navigation/NavbarTop.vue'

</script>
