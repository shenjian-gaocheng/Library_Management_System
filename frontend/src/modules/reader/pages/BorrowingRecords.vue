<template>
  <div class="layout">
    <!-- 左侧侧边栏 -->
    <DashBoardSidebar />

    <!-- 右侧内容区域 -->
    <div class="main-content">
      <!-- 顶部导航栏 -->
      <DashBoardNavbar />

      <!-- 页面内容 -->
      <div class="container">
        <h1 class="title">📚 我的借阅记录</h1>

        <div class="actions">
          <button class="btn-refresh" @click="loadRecords">🔄 刷新记录</button>
        </div>

        <div class="card">
          <BorrowingRecordTable :records="records" :pageSize="7" />
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue';
import BorrowingRecordTable from '@/modules/reader/components/BorrowingRecordTable.vue';
import { getAllBorrowingRecords } from '@/modules/reader/api.js';
import DashBoardSidebar from '@/modules/reader/components/DashBoardSidebar.vue';
import DashBoardNavbar from '@/modules/reader/components/DashBoardNavbar.vue';

const records = ref([]);

// 加载借阅记录
const loadRecords = async () => {
  try {
    const res = await getAllBorrowingRecords();
    records.value = res.data || [];
  } catch (error) {
    console.error('获取借阅记录失败', error);
    alert('获取失败，请检查登录状态或接口地址');
  }
};

onMounted(loadRecords);
</script>

<style scoped>
/* 页面整体布局：左右两栏 */
.layout {
  display: flex;
  min-height: 100vh;
}

/* 左侧侧边栏由 DashBoardSidebar 控制宽度 */

/* 右侧内容 */
.main-content {
  flex: 1;
  background-color: #f8f9fa;
  display: flex;
  flex-direction: column;
}

/* Navbar 顶部 */
.main-content > *:first-child {
  flex-shrink: 0;
}

/* 页面内容容器 */
.container {
  max-width: 900px;
  margin: 24px auto 0 auto; /* 顶部距离 navbar */
  padding: 0 16px;
}

/* 页面标题 */
.title {
  text-align: center;
  margin-bottom: 20px;
  color: #2c3e50;
  font-size: 28px;
  font-weight: bold;
}

/* 操作区域 */
.actions {
  text-align: right;
  margin-bottom: 15px;
}

/* 刷新按钮 */
.btn-refresh {
  background: #3498db;
  color: white;
  padding: 8px 16px;
  border-radius: 6px;
  border: none;
  cursor: pointer;
  font-size: 14px;
  transition: 0.3s;
}
.btn-refresh:hover {
  background: #2980b9;
}

/* 表格卡片 */
.card {
  background: #fff;
  padding: 20px;
  border-radius: 10px;
  box-shadow: 0px 4px 12px rgba(0,0,0,0.08);
  transition: 0.3s;
}
.card:hover {
  box-shadow: 0px 6px 18px rgba(0,0,0,0.12);
}
</style>
