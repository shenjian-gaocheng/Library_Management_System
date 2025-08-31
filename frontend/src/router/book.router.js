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

  // 添加首页头部导航栏 "我的图书馆" 中 "我的书单主页面" 路由
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
  },

  {
    path: '/user/booklist/created/:id',
    name: 'BooklistCreated',
    component: () =>
      import(
        /* webpackChunkName: "book-booklist" */
        '@/modules/book/pages/BooklistDetailCreated.vue'
      ),
    meta: {
      requiresAuth: true, title: '我创建的书单' }
  },

  {
    path: '/user/booklist/collected/:id',
    name: 'BooklistCollected',
    component: () =>
      import(
        /* webpackChunkName: "book-booklist" */
        '@/modules/book/pages/BooklistDetailCollected.vue'
      ),
    meta: {
      requiresAuth: true, title: '我收藏的书单' }
  },

  {
    path: '/user/booklist/recommend',
    name: 'BooklistRecommend',
    component: () =>
      import(
        /* webpackChunkName: "book-booklist" */
        '@/modules/book/pages/BooklistRecommend.vue'
      ),
    meta: {
      requiresAuth: true, title: '推荐书单' }
  }
]