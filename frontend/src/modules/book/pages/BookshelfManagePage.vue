<template>
  <!-- 移除了外层的 <LayoutDefault> 包装 -->
  <div class="page-container">
    <!-- 左侧切换按钮 -->
    <div class="side-buttons">
      <button 
        @click="activePage = 'bookshelf'" 
        :class="{ active: activePage === 'bookshelf' }"
      >
        书籍管理
      </button>
      <button 
        @click="activePage = 'shelf-manage'" 
        :class="{ active: activePage === 'shelf-manage' }"
      >
        书架管理
      </button>
    </div>
    
    <!-- 右侧内容区 -->
    <div class="content-area">
      <BookshelfManage v-if="activePage === 'bookshelf'" />
      <BookshelfManage2 v-if="activePage === 'shelf-manage'" />
    </div>
  </div>
</template>

<script setup>
import { ref } from 'vue';
// 移除了对 LayoutDefault 的导入
import BookshelfManage from '@/modules/book/components/BookshelfManage.vue';
import BookshelfManage2 from '@/modules/book/components/BookshelfManage2.vue';

const activePage = ref('bookshelf');
</script>

<style scoped>
.page-container {
  display: flex;
  /* 调整高度以适应父容器，而不是视口 */
  height: calc(100vh - 68px); /* 视口高度减去管理员头部高度 (大约值，可微调) */
}

.side-buttons {
  width: 150px;
  padding: 20px 10px;
  background-color: #ffffff; /* 推荐使用白色或与主题匹配的颜色 */
  box-shadow: 1px 0 3px rgba(0, 0, 0, 0.05);
  display: flex;
  flex-direction: column;
  gap: 10px;
  height: 100%; /* 撑满父容器 .page-container 的高度 */
}

.side-buttons button {
  padding: 10px;
  border: none;
  background: #f0f4f8; /* 使用管理员背景色，更和谐 */
  cursor: pointer;
  border-radius: 4px;
  text-align: center;
  color: #334155;
  font-weight: 600;
}

.side-buttons button.active {
  background: #3b82f6; /* 使用管理员主题色 */
  color: white;
}

.side-buttons button:hover:not(.active) {
  background: #e2e8f0; /* 悬停颜色 */
}

.side-buttons button.active:hover {
  background: #2563eb;
}

.content-area {
  flex: 1;
  padding: 20px;
  overflow-y: auto; /* 如果内容超长，允许滚动 */
  height: 100%;
}
</style>