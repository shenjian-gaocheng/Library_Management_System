import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '@/modules/home/pages/HomeView.vue'

import adminRoutes  from '@/router/admin.routes.js'
import bookRoutes   from '@/router/book.router.js'
// import readerRoutes from '@/router/reader.routes.js'

const routes = [
  { path: '/', name: 'HomeView', component: HomeView },
  ...adminRoutes,
  ...bookRoutes,
  // ...readerRoutes
]

const router = createRouter({
  history: createWebHistory(),
  routes
})

export default router