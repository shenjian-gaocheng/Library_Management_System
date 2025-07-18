<!-- BookSearch.vue -->
<script setup>
import { ref } from 'vue'
import { getBooks } from '../api.js'  // 根据你的模块路径，调整为相对路径

const keyword = ref('')
const books = ref([])

const search = async () => {
  const res = await getBooks(keyword.value)
  books.value = res.data
}
</script>

<template>
  <div class="book-search">
    <input v-model="keyword" placeholder="请输入书名或作者" />
    <button @click="search">搜索</button>

    <div v-for="book in books" :key="book.BookID">
      <p>{{ book.Title }} - {{ book.Author }}</p>
    </div>
  </div>
</template>

<style scoped>
.book-search {
  padding: 1rem;
}
input {
  margin-right: 0.5rem;
}
</style>
