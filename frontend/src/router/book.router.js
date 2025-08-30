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
    name: 'Booklist',
    component: () =>
      import(
        /* webpackChunkName: "book-booklist" */
        '@/modules/book/pages/BooklistPage.vue'
      ),
    meta: {
      requiresAuth: true, title: '我的书单' }
  }
]