// router/reader.router.js

export default [
  //导出我的图书馆页面路由
  {
    path: '/my/home/dashboard',
    name: 'my-home-dashboard',
    component: () => import('@/modules/reader/pages/DashBoardPage.vue'),
    meta: { requiresAuth: true }
  },
];
