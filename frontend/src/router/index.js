// 文件: frontend/src/router/index.js
// 这是最终的、使用了正确嵌套布局的路由配置

import { createRouter, createWebHistory } from 'vue-router';

// 导入我们的布局“画框”/主页
import HomeView from '@/modules/home/pages/HomeView.vue';

// 导入我们的子路由模块
import adminRoutes from './admin.routes.js'; 
import bookRoutes from './book.router.js';

const routes = [
  { 
    // 定义我们的主布局路由
    path: '/', 
    component: HomeView, // 所有子路由都会被渲染在 HomeView 的 <router-view> 中
    
    // 当用户访问根路径 "/" 时，自动跳转到“首页”的实际内容页面
    redirect: '/home', 
    
    // 定义所有使用这个布局的子页面
    children: [
      // 【新增】为主页本身创建一个子路由
      // 这样 HomeView 内部的 <router-view> 就有内容可以渲染了
      {
        path: '/home',
        name: 'HomePageContent',
        component: () => import('@/modules/home/components/HomePageContent.vue') // 我们将创建一个新组件
      },

      // 使用展开运算符，将 admin 和 book 的路由都添加为子路由
      ...adminRoutes,
      ...bookRoutes,
    ]
   },
   // 在这里可以定义其他不需要主布局的顶层页面，例如登录页
   // {
   //   path: '/login', ...
   // }
];

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes
});

export default router;