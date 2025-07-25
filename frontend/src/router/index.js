import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '@/shared/pages/HomeView.vue'

import adminRoutes from './admin.routes.js'; 

const routes = [
  { path: '/', name: 'HomeView', component: HomeView,

    
    children: [
      // ... 这里可能未来还有 book.routes, reader.routes 等
      ...adminRoutes, // 使用展开运算符(...)将 adminRoutes 数组的所有项添加到 children 中
    ]
   },
 
]

const router = createRouter({
  history: createWebHistory(),
  routes
})

export default router
