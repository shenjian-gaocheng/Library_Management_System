import AdminDashboardPage from '@/modules/admin/pages/AdminDashboardPage.vue'
import CategoryManagePage from '@/modules/book/pages/CategoryManagePage.vue'
import PurchaseAnalyticsPage from '@/modules/admin/pages/PurchaseAnalyticsPage.vue'
import AnnouncementManagePage from '@/modules/admin/pages/AnnouncementManagePage.vue'
import BookManagePage from '@/modules/admin/pages/BookManagePage.vue'
import ShelfManagePage from '@/modules/admin/pages/ShelfManagePage.vue' // 确保这个新页面被引入
import ReportHandlingPage from '@/modules/admin/pages/ReportHandlingPage.vue'

export default [
  // 管理员仪表盘
  {
    path: '/admin/dashboard',
    name: 'AdminDashboard',
    component: AdminDashboardPage,
    meta: {
      layout: 'Admin',
      requiresAuth: true,
      requiresAdmin: true,
      title: '管理员仪表盘'
    }
  },

  // 分类管理
  {
    path: '/admin/category',
    name: 'CategoryManage',
    component: CategoryManagePage,
    meta: {
      layout: 'Admin',
      requiresAuth: true,
      requiresAdmin: true,
      title: '分类管理'
    }
  },

  // 采购分析
  {
    path: '/admin/analytics',
    name: 'PurchaseAnalytics',
    component: PurchaseAnalyticsPage,
    meta: {
      layout: 'Admin',
      requiresAuth: true,
      requiresAdmin: true,
      title: '采购分析'
    }
  },



  // 公告发布
  {
    path: '/admin/announcements',
    name: 'AnnouncementManage',
    component: AnnouncementManagePage,
    meta: {
      layout: 'Admin',
      requiresAuth: true,
      requiresAdmin: true,
      title: '公告发布'
    }
  },
  
  // 图书管理
  {
    path: '/admin/books',
    name: 'BookManage',
    component: BookManagePage,
    meta: {
      layout: 'Admin',
      requiresAuth: true,
      requiresAdmin: true,
      title: '图书管理'
    }
  },
  
  // 书架管理 (新的、正确的路由)
  {
    path: '/admin/shelves',
    name: 'ShelfManage',
    component: ShelfManagePage,
    meta: {
      layout: 'Admin',
      requiresAuth: true,
      requiresAdmin: true,
      title: '书架管理'
    }
  },

  {
    path: '/admin/reports',
    name: 'ReportHandling',
    component: ReportHandlingPage,
    meta: {       
      layout: 'Admin', 
      requiresAuth: true,
      requiresAdmin: true,
      title: '举报处理' }
  }
]