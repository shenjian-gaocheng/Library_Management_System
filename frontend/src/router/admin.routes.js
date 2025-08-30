import CategoryManagePage from '@/modules/book/pages/CategoryManagePage.vue'

export default [
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
