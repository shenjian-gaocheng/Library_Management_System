// 文件: frontend/src/router/index.js
// 这是最终的、最规范的路由配置文件

import { createRouter, createWebHistory } from 'vue-router';

// 导入我们的布局“画框”
import HomeView from '@/shared/pages/HomeView.vue';

// 导入我们所有的管理员子路由
import adminRoutes from './admin.routes.js'; 

const routes = [
  { 
    // 定义我们的主布局路由
    path: '/', 
    component: HomeView, // 所有子路由都会被渲染在 HomeView 的 <router-view> 中
    
    // 当用户访问根路径 "/" 时，自动跳转到“账户管理”页面
    redirect: '/admin/librarians', 
    
    // 定义所有使用这个布局的子页面
    children: [
      // 在这里可以定义其他非管理员的子页面，例如：
      // {
      //   path: '/overview',
      //   name: 'Overview',
      //   component: () => import('@/modules/overview/pages/OverviewPage.vue')
      // },

      // 使用展开运算符，将 adminRoutes 数组中的所有路由对象都添加为这里的子路由
      ...adminRoutes,
    ]
   },
   // 在这里可以定义其他不需要主布局的顶层页面，例如登录页
   // {
   //   path: '/login',
   //   name: 'Login',
   //   component: () => import('@/pages/LoginPage.vue')
   // }
];

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes
});

export default router;