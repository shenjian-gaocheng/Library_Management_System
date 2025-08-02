import BookBooklistPage from '@/modules/book/pages/BooklistPage.vue'

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
   //书架管理
  {
    path: '/bookshelf',
    name: 'BookshelfManage',
    component: () =>
    import(
        /* webpackChunkName: "book-search" */
        '@/modules/book/pages/BookshelfManagePage.vue'
    ),
    meta: {
      requiresAuth: true,
      title: '书架管理',
      //role: 'librarian' // 添加权限控制，仅管理员可见
    }
  }
]