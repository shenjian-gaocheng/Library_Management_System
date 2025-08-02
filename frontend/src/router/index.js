// router/index.js

import { createRouter, createWebHistory } from 'vue-router';
import readerRoutes from './reader.routes.js';
import {jwtDecode} from 'jwt-decode';
// import adminRoutes from './admin.routes.js';
// import bookRoutes from './book.routes.js';

const routes = [
  {
    path: '/',
    name: 'HomeView',
    component: () => import('@/shared/pages/HomeView.vue')
  },
  {
    path: '/auth',
    name: 'AuthPage',
    component: () => import('@/shared/pages/AuthPage.vue')
  },
  ...readerRoutes,
  //...adminRoutes,
 // ...bookRoutes,
];

const router = createRouter({
  history: createWebHistory(),
  routes
});

//判断token是否存在且未过有效期
function isLoggedIn() {
  const token = localStorage.getItem('token');
  if (!token) return false;

    const { exp } = jwtDecode(token);
    const now = Math.floor(Date.now() / 1000);
    return exp && exp > now;
}

router.beforeEach((to, from, next) => {
  if (to.meta.requiresAuth && !isLoggedIn()) {
    next({
      path: '/auth',
      query: { redirect: to.fullPath }
    });
  } else {
    next();
  }
});


export default router;
