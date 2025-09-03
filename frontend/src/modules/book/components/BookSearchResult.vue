<template>
  <section class="search-result-wrapper bg-gray-100">
    <h2 class="title">搜索结果："{{ keyword }}"</h2>

    <div v-if="loading" class="loading">加载中...</div>

    <div v-else-if="error" class="error">{{ error }}</div>

    <div v-else>
      <p v-if="books.length === 0" class="empty">未找到相关图书</p>
      <ul v-else class="flex flex-wrap gap-6 justify-center">
        <div v-for="book in books" :key="book.ISBN" class="wr_suggestion_card_wrapper relative" >
          <div class="wr_suggestion_card_content pb-16">
            <img
              :src="`/covers/${book.ISBN}.jpg`"
              alt="封面"
              class="fixed-cover-size"
              @error="e => (e.target.src = defaultCover)"
            />
            <div class="mt-4 text-center ">
              <div class="text-base font-semibold leading-tight ">
                {{ book.Title }}
              </div>
              <div class="text-sm text-gray-600 mt-1 ">
                作者：{{ book.Author }}
              </div>
                   
            </div>
          </div>

                  <div class="absolute bottom-4   flex flex-col gap-2 items-center ">
                    <button 
                      @click="viewComments(book.ISBN)"
                      class="comments-button "
                    >
                      查看评论
                    </button>
                    <button 
                      @click="viewPhysicalBooks(book.Title)"
                      class="physical-books-button "
                    >
                      查看实体书
                    </button>
                  </div>
        </div>
      </ul>
    </div>
  </section>
</template>

<script setup>
import { ref, onMounted, watch } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { getBooks } from '@/modules/book/api.js'

const route = useRoute()
const router = useRouter()
const keyword = ref(route.query.q || '')
const books = ref([])
const loading = ref(false)
const error = ref('')

// 默认封面
const defaultCover = new URL('@/assets/book_cover_default.jpg', import.meta.url).href

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

// 查看评论功能
function viewComments(isbn) {
  router.push({
    path: '/comments',
    query: { isbn: isbn }
  })
}

//实体书
// 查看实体书功能
function viewPhysicalBooks(title) {
  router.push({
    path: '/booklocation',
    query: { q: title }
  })
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
  position: relative;
  margin: 2rem auto;
  padding: 0 12rem;
  color:#666;
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


.wr_suggestion_card_wrapper {
  background: #ffffff; 
  border: 1px solid #e5e7eb; /* 添加浅灰色边框 */
  border-radius: 12px;
  box-sizing: border-box;
  cursor: pointer;
  padding: 40px 20px;
  transition: transform 0.3s;
  width: 282px;
  display: flex;
  flex-direction: column;
  align-items: center;
}

.comments-button {
  background-color: #409eff;
  color: white;
  border: none;
  border-radius: 4px;
  padding: 6px 8px;
  cursor: pointer;
  font-size: 14px;
  transition: background-color 0.3s;
}

.comments-button:hover {
  background-color: #337ecc;
}



.physical-books-button {
  background-color: #67c23a;
  color: white;
  border: none;
  border-radius: 4px;
  padding: 6px 8px;
  cursor: pointer;
  font-size: 14px;
  transition: background-color 0.3s;
}

.physical-books-button:hover {
  background-color: #85ce61;
}

.comments-button, .physical-books-button {
  width: 120px;  /* 或者具体的像素值，如 width: 80px; */
  
}
</style>