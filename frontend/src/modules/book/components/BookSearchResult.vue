<template>
  <section class="search-result-wrapper">
    <h2 class="title">搜索结果："{{ keyword }}"</h2>

    <div v-if="loading" class="loading">加载中...</div>

    <div v-else-if="error" class="error">{{ error }}</div>

    <div v-else>
      <p v-if="books.length === 0" class="empty">未找到相关图书</p>
      <ul v-else class="book-list">
        <li v-for="book in books" :key="book.BookID" class="book-card">
          <h3>{{ book.Title }}</h3>
          <p class="author">作者：{{ book.Author }}</p>
        </li>
      </ul>
    </div>
  </section>
</template>

<script setup>
import { ref, onMounted, watch } from 'vue'
import { useRoute } from 'vue-router'
import { getBooks } from '@/modules/book/api.js'

const route = useRoute()
const keyword = ref(route.query.q || '')
const books = ref([])
const loading = ref(false)
const error = ref('')

async function fetchBooks() {
  if (!keyword.value) return
  loading.value = true
  error.value = ''
  try {
    const res = await getBooks(keyword.value)
    books.value = res.data || []
  } catch (e) {
    error.value = '搜索失败，请稍后重试'
  } finally {
    loading.value = false
  }
}

// 初次加载
onMounted(fetchBooks)

// 监听路由 query 变化，支持在结果页继续搜索
watch(
  () => route.query.q,
  newQ => {
    keyword.value = newQ || ''
    fetchBooks()
  }
)
</script>

<style scoped>
.search-result-wrapper {
  position: absolute;
  max-width: 900px;
  margin: 2rem auto;
  padding: 0 1rem;
}
.title {
  font-size: 1.25rem;
  font-weight: 700;
  margin-bottom: 1.5rem;
}
.loading,
.error,
.empty {
  text-align: center;
  font-size: 1rem;
  padding: 2rem 0;
  color: #666;
}
.book-list {
  list-style: none;
  padding: 0;
  margin: 0;
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(260px, 1fr));
  gap: 1rem;
}
.book-card {
  background: #fff;
  border: 1px solid #e5e7eb;
  border-radius: 8px;
  padding: 1rem;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.05);
}
.book-card h3 {
  margin: 0 0 0.5rem 0;
  font-size: 1.1rem;
}
.book-card .author {
  font-size: 0.9rem;
  color: #555;
  margin-bottom: 0.5rem;
}
.book-card .desc {
  font-size: 0.85rem;
  color: #666;
  max-height: 4.5em;
  overflow: hidden;
  text-overflow: ellipsis;
}
</style>
