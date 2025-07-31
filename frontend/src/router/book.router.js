import BookBooklistPage from '@/modules/book/pages/BooklistPage.vue'
import BookshelfManagePage from '@/modules/book/pages/BookshelfManagePage.vue' // 新增的书架管理组件
export default [
  // 图书搜索结果页
  {
    path: '/search',
    name: 'SearchResult',
    component: () =>
      import(
        /* webpackChunkName: "book-search" */
        '@/modules/book/pages/BookSearchResultPage.vue'
      ),
    meta: { requiresAuth: false, title: '图书搜索' }
  },

  // 添加首页头部导航栏 "我的图书馆" 中 "我的书单" 路由
  {
    path: '/user/booklist',
    name: 'BookBooklist',
    component: BookBooklistPage,
    meta: {
      requiresAuth: true,
      title: '我的书单'
    }
  },
   // 新增书架管理（与我的书单并列）
  {
    path: '/user/bookshelf',
    name: 'BookshelfManage',
    component: BookshelfManagePage,
    meta: {
      requiresAuth: true,
      title: '书架管理',
      //role: 'librarian' // 添加权限控制，仅管理员可见
    }
  }
]