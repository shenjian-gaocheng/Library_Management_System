import  BorrowingTest  from '@/modules/reader/components/BorrowingTest.vue';

export default [
  //导出我的图书馆页面路由
  {
    path: '/my/home/dashboard',
    name: 'my-home-dashboard',
    component: () => import('@/modules/reader/pages/DashBoardHome_Page.vue'),
    meta: { requiresAuth: true }
  },
  {
    path: '/my/borrowingRecords',
    name: 'BorrowingRecords',
    component: () => import('@/modules/reader/pages/BorrowingRecords.vue'),
  },
 // 其他reader模块路由...
  {
    path: '/reader/BorrowingTest',
    name: 'BorrowingTest',
    component: BorrowingTest,
    meta: {
      title: '借阅记录管理',
      //requiresAuth: true // 如果需要登录验证
    }
  },

  {
    path: '/space/seats',
    name: 'SeatReservation',
    component: () => import('@/modules/reader/pages/SeatReservationPage.vue'),
    meta: { requiresAuth: true, title: '座位预约' }
  },

  {
    path: '/my/reservations',
    name: 'MyReservations',
    component: () => import('@/modules/reader/pages/MyReservationsPage.vue'),
    meta: { requiresAuth: true, title: '我的预约', layout: 'Dashboard' } // 假设读者有自己的控制台布局
  }
];
