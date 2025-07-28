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
]