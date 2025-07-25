// 文件: frontend/src/router/admin.routes.js
export default [
  {
    path: '/admin/librarians', // 使用一个清晰的路径
    name: 'AdminLibrarianManagement',
    // 使用懒加载方式引入页面组件
    component: () => import('@/modules/admin/pages/LibrarianManagementPage.vue')
  }
  // ... 其他管理员相关的路由
];