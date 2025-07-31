import { BorrowingTest } from '@/modules/reader'; 

export default [
  // 其他reader模块路由...
  {
    path: '/reader/borrowing',
    name: 'BorrowingTest',
    component: BorrowingTest,
    meta: {
      title: '借阅记录管理',
      //requiresAuth: true // 如果需要登录验证
    }
  }
];