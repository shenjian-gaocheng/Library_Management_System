<template>
  <nav class="navbar">
    <div class="title">
      图书管理系统
    </div>

    <ul class="nav-list">
      <li
        v-for="(item, i) in navItems"
        :key="item.path"
        class="nav-item"
        @mouseover="showDropdown(i)"
        @mouseleave="hideDropdown(i)"
      >
        <!-- main link -->
        <router-link
          :to="item.path"
          class="nav-link"
          :class="{ active: isActive(item.path) }"
        >
          {{ item.label }}
        </router-link>

        <!-- dropdown menu -->
        <DropdownMenu
          :children="item.children"
          :isVisible="isDropdownVisible[i]"
        />

        <!-- vertical separator -->
        <span v-if="i !== navItems.length - 1" class="separator" aria-hidden="true">|</span>
      </li>
    </ul>
     <!-- 添加图片 -->
    <div class="navbar-image">
       <img src="/src/assets/test.png" alt="图书管理系统 Logo" style="width: 100px; height: 100px; object-fit: cover;">
    </div>
  </nav>
</template>

<script setup lang="ts">
import { useRoute } from 'vue-router'
import { ref } from 'vue'
import DropdownMenu from './DropdownMenu.vue'

/**
 * Navigation items configuration
 */
const navItems = [
  { path: '/', label: '首页' },
  { path: '/overview', label: '图书馆概况' },
  { path: '/resources', 
    label: '文献资源' ,
    children:[
      {path:'/resources/query',label:'文献查找'},
    ]
  },
   {
    path: '/services',
    label: '读者服务',
    children: [
      { path: '/auth', label: '我的图书馆' },
      { path: '/services/study-room', label: '自习室预约' },
      { path: '/services/return', label: '还书服务' },
    ],
  },
  { path: '/research', label: '科研支持' },
  { path: '/culture', label: '文化活动' },
  { path: '/community', label: '互动交流' },
] as const

type NavItem = (typeof navItems)[number]

const route = useRoute()

const isDropdownVisible = ref(navItems.map(() => false))

function showDropdown(index: number) {
  isDropdownVisible.value[index] = true
}

function hideDropdown(index: number) {
  isDropdownVisible.value[index] = false
}

function isActive(path: NavItem['path']) {
  if (path === '/') return route.path === '/'
  return route.path.startsWith(path)
}
</script>

<style scoped>
.title {
  background: #00559a;
  color: #fff;
  font-size: 1.7rem;          /* ≈ text-2xl */
  font-weight: 600;
  letter-spacing: .05em;
  font-family:
    'Noto Serif SC',
    'Source Han Serif SC',
    'Microsoft YaHei',
    'PingFang SC',
    sans-serif;
  padding: 0.75rem 1.5rem;         /* 上下 12 px */
}
/* ----- layout ----- */
.navbar {
  width: 100%;
  margin: 0 auto;
  background: #00559a; /* 深蓝 */
  box-shadow: 0 1px 2px rgba(0, 0, 0, 0.06);
}

.nav-list {
  display: flex;
  justify-content: center;
  align-items: center;
  list-style: none;
  margin: 0;
  padding: 0;
}

.nav-item {
  display: flex;
  align-items: center;
  position: relative; 
}

/* ----- link style ----- */
.nav-link {
  white-space: nowrap; 
  display: block;
  padding: 1rem 2.25rem;
  color: #ffffff;
  text-decoration: none;
  font-size: 1.2rem;
  line-height: 1;
  transition: background 0.2s ease, color 0.2s ease;
  border-radius: 3px;
}

.nav-link:hover {
  background: #ffffff;
  color: #00559a;
  border-radius: 3px;
}

.nav-link.active {
  background: #ffffff;
  color: #00559a;
  border-radius: 3px;
}

/* ----- separator ----- */
.separator {
  margin: 0 0.9rem;
  color: #ffffff;
  user-select: none;
}

/* ----- navbar image ----- */
.navbar-image {
  position: absolute; /* 使用绝对定位 */
  top: 0;           /* 顶部对齐 */
  right: 2.5rem;         /* 右侧对齐 */
  padding: 0.75rem; /* 添加一些内边距 */
  z-index: 10; /* 设置较高的 z-index 值以确保图片在最上方 */
}
</style>
