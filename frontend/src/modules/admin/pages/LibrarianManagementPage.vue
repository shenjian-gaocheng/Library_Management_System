<script setup>
import { ref, onMounted } from 'vue';
// 1. 导入API函数
import { getLibrarians, createLibrarian, deleteLibrarian } from '../api.js';

import LibrarianList from '../components/LibrarianList.vue';
import LibrarianAddForm from '../components/LibrarianAddForm.vue';
import LibrarianSearchForm from '../components/LibrarianSearchForm.vue';

const librarians = ref([]);
const isLoading = ref(true);
const currentKeyword = ref('');

// 获取管理员列表的逻辑（由页面负责）
const fetchLibrarians = async () => {
  isLoading.value = true;
  try {
    // 将当前关键词传递给 API 函数
    const response = await getLibrarians(currentKeyword.value);
    librarians.value = response.data;
  } catch (error) {
    alert('错误：无法从服务器获取管理员列表。');
    console.error("获取管理员列表失败:", error);
  } finally {
    isLoading.value = false;
  }
};

// 组件加载后自动获取数据
onMounted(fetchLibrarians);

const handleSearch = (keyword) => {
  currentKeyword.value = keyword;
  fetchLibrarians(); // 使用新的关键词重新获取数据
};

// 处理来自 LibrarianAddForm 组件的 'add-librarian' 事件
const handleAddLibrarian = async (formData) => {
  try {
    await createLibrarian(formData);
    alert(`管理员 "${formData.name}" 新增成功！`);
    // 【优化点】: 新增后应该清空搜索条件，并加载全部数据
    currentKeyword.value = ''; 
    await fetchLibrarians();
  } catch (error) {
    alert('错误：新增管理员失败。可能是ID重复或服务器错误。');
    console.error("新增管理员失败:", error);
  }
};

// 处理来自 LibrarianList 组件的 'delete-librarian' 事件
const handleDeleteLibrarian = async (id) => {
  try {
    await deleteLibrarian(id);
    alert(`ID为 "${id}" 的管理员已成功删除。`);
    // 【优化点】: 删除后也应该用当前的搜索条件刷新
    await fetchLibrarians();
  } catch (error) {
    alert('错误：删除管理员失败。');
    console.error("删除管理员失败:", error);
  }
};
</script>

<template>
  <div class="admin-page-container">
    <header>
      <h1>管理员账户管理</h1>
    </header>
    <main>
      <!-- 将新增和查询表单放在一个容器里，用于布局 -->
      <div class="form-container">
        <LibrarianAddForm @add-librarian="handleAddLibrarian" />
        <!-- 5. 在页面中使用查询组件，并监听其 search 事件 -->
        <LibrarianSearchForm @search="handleSearch" />
      </div>
      
      <LibrarianList 
        :librarians="librarians" 
        :is-loading="isLoading"
        @delete-librarian="handleDeleteLibrarian" 
      />
    </main>
  </div>
</template>

<style scoped>
.admin-page-container {
  padding: 2rem 3rem;
  font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
  background-color: #f8f9fa; /* 给页面一个浅灰色背景 */
}
header h1 {
  font-size: 2rem;
  font-weight: 300;
  color: #333;
  border-bottom: 1px solid #dee2e6;
  padding-bottom: 1rem;
  margin-bottom: 2rem;
}
/* 新增的容器样式 */
.form-container {
  background: #ffffff;
  padding: 1.5rem;
  border-radius: 8px;
  box-shadow: 0 2px 4px rgba(0,0,0,0.05);
  margin-bottom: 2rem;
}
</style>