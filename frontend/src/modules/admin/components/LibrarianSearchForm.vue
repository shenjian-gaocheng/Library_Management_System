<!-- 文件: frontend/src/modules/admin/components/LibrarianSearchForm.vue -->
<script setup>
import { ref } from 'vue';

const keyword = ref('');
const emit = defineEmits(['search']);

const handleSearch = () => {
  // 触发 search 事件，把搜索关键词传出去
  emit('search', keyword.value);
};

const clearSearch = () => {
  keyword.value = '';
  emit('search', ''); // 清空时也触发一次搜索，以加载所有数据
}
</script>

<template>
  <div class="search-form">
    <input 
      v-model.trim="keyword" 
      placeholder="按ID或姓名搜索..." 
      @keyup.enter="handleSearch"
    />
    <button @click="handleSearch">查询</button>
    <button @click="clearSearch" class="clear-btn" v-if="keyword">清空</button>
  </div>
</template>

<style scoped>
.search-form {
  display: flex;
  gap: 10px;
  margin-bottom: 1.5rem;
}
input {
  flex-grow: 1;
  padding: 10px;
  border: 1px solid #ccc;
  border-radius: 4px;
}
button {
  padding: 10px 20px;
  border: none;
  border-radius: 4px;
  background-color: #28a745;
  color: white;
  cursor: pointer;
}
.clear-btn {
  background-color: #6c757d;
}
</style>