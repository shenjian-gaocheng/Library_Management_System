// 文件: frontend/src/router/admin.routes.js
// 这个文件现在包含了所有管理员相关的路由
﻿import CategoryManagePage from '@/modules/book/pages/CategoryManagePage.vue'

export default [
  {
    path: '/admin/announcements',
    name: 'AdminAnnouncementManagement',
    component: () => import('@/modules/admin/pages/AnnouncementManagementPage.vue')
  },

  // 分类管理页面
  {
    path: '/admin/category',
    name: 'CategoryManage',
    component: CategoryManagePage,
    meta: {
      requiresAuth: true,
      requiresAdmin: true,
      title: '分类管理'
    }
  }
]
