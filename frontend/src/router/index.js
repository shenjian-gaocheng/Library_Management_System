// 文件: frontend/src/router/index.js
// 这是最终的、使用了正确嵌套布局的路由配置

import { createRouter, createWebHistory } from 'vue-router';

// 导入我们的布局“画框”/主页
import HomeView from '@/modules/home/pages/HomeView.vue';

导入我们的子路由模块
import adminRoutes from './admin.routes.js'; 
import bookRoutes from './book.router.js';

const routes = [
  { path: '/', name: 'HomeView', component: HomeView },
  // ...adminRoutes,
  ...bookRoutes,
  // ...readerRoutes
]

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes
});

export default router;