import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '@/shared/pages/HomeView.vue'
import readerRoutes from '@/router/reader.routes.js'
import AuthPage from '@/shared/pages/AuthPage.vue'

const routes = [
  { path: '/', name: 'HomeView', component: HomeView },
  { path: '/auth', name: 'AuthPage',component: AuthPage},
  ...readerRoutes,

]

const router = createRouter({
  history: createWebHistory(),
  routes
})

export default router
