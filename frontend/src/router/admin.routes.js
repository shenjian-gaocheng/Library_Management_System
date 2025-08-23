// 文件: frontend/src/router/admin.routes.js
// 这个文件现在包含了所有管理员相关的路由

export default [
  {
    path: '/admin/announcements',
    name: 'AdminAnnouncementManagement',
    component: () => import('@/modules/admin/pages/AnnouncementManagementPage.vue')
  }
];