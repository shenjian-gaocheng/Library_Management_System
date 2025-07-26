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
      >
        <!-- main link -->
        <router-link
          :to="item.path"
          class="nav-link"
          :class="{ active: isActive(item.path) }"
        >
          {{ item.label }}
        </router-link>

        <!-- vertical separator -->
        <span v-if="i !== navItems.length - 1" class="separator" aria-hidden="true">|</span>
      </li>
    </ul>
  </nav>
</template>

<script setup lang="ts">
import { useRoute } from 'vue-router'

/**
 * Navigation items configuration
 */
const navItems = [
  { path: '/', label: '首页' },
  { path: '/overview', label: '图书馆概况' },
  { path: '/resources', label: '文献资源' },
  { path: '/services', label: '读者服务' },
  { path: '/research', label: '科研支持' },
  { path: '/culture', label: '文化活动' },
  { path: '/community', label: '互动交流' },

  // --- 管理员专属链接 ---
  { path: '/admin/librarians', label: '账户管理' }, // 您已有的
  { path: '/admin/announcements', label: '公告管理' } // <-- 【新增】在这里添加这一行
] as const

type NavItem = (typeof navItems)[number]

const route = useRoute()

function isActive(path: NavItem['path']) {
  if (path === '/') return route.path === '/'
  return route.path.startsWith(path)
}
</script>

<style scoped>
.title {
  background: #00559a;
  color: #fff;
  font-size: 1.5rem;          /* ≈ text-2xl */
  font-weight: 600;
  letter-spacing: .05em;
  font-family:
    'Noto Serif SC',
    'Source Han Serif SC',
    'Microsoft YaHei',
    'PingFang SC',
    sans-serif;
  padding: 0.75rem 0;         /* 上下 12 px */
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
}

/* ----- link style ----- */
.nav-link {
  white-space: nowrap; 
  display: block;
  padding: 1rem 2.25rem;
  color: #ffffff;
  text-decoration: none;
  font-size: 1rem;
  line-height: 1;
  transition: background 0.2s ease, color 0.2s ease;
}

.nav-link:hover {
  background: #ffffff;
  color: #00559a;
}

.nav-link.active {
  background: #ffffff;
  color: #00559a;
}

/* ----- separator ----- */
.separator {
  margin: 0 0.9rem;
  color: #ffffff;
  user-select: none;
}
</style>
