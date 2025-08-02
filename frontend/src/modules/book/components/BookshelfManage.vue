<template>
  <p class="search-tip">请输入要查询或加入书架的书籍</p>

  <div class="simple-search">
    
    <input
      v-model="searchText"
      type="text"
      placeholder="搜索图书..."
      
    />
    <button @click="handleSearch">
      <i class="search-icon">搜索</i>
    </button>
  </div>
  
  <!-- 搜索结果展示区域 -->
      <!-- 简化后的搜索结果展示区域 -->
  <div v-if="searchPerformed" class="search-results">
    <div v-if="loading" class="loading">搜索中...</div>
    
    <div v-else-if="error" class="error">{{ error }}</div>
    
    <div v-else>
      <div v-if="foundBooks.length === 0" class="no-results">
        未找到"{{ searchText }}"的相关书籍
      </div>
      
      <div v-else class="results-list">
        <h3 class="results-title">搜索结果 (共{{ foundBooks.length }}条)</h3>
        <table class="books-table">
          <thead>
            <tr>
              <th>书名</th>
              <th>所在书架</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="book in foundBooks" :key="book.title">
              <td>《{{ book.TITLE }}》</td>
              <td>
                <template v-if="book.SHELFID">
                  {{ book.SHELFID }}
                  </template>
                <template v-else>
                   {无书架信息}
                </template>

              </td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>
  </div>

 


   <div class="location-form">
    <h3 class="form-title">要放置的位置</h3>
    
    <div class="form-item">
      <label>所属楼宇 ID：</label>
      <input 
        v-model="location.buildingId" 
        type="text" 
        placeholder="请输入楼宇ID"
      >
    </div>
    
    <div class="form-item">
      <label>书架编码：</label>
      <input 
        v-model="location.shelfCode" 
        type="text" 
        placeholder="请输入书架业务标识"
      >
    </div>
    
    <div class="form-item">
      <label>所在楼层：</label>
      <input 
        v-model="location.floor" 
        type="text" 
        placeholder="请输入楼层"
      >
    </div>
    
    <div class="form-item">
      <label>所在区域：</label>
      <input 
        v-model="location.zone" 
        type="text" 
        placeholder="请输入区域"
      >
    </div>
  </div>
</template>

<script setup>
import { ref, reactive } from 'vue'
import axios from 'axios'
import {  getBooks,getBooksBookShelf } from '@/modules/book/api.js'
const searchText = ref('')
const loading = ref(false)
const error = ref('')
const searchPerformed = ref(false) // 标记是否已执行过搜索
const foundBooks = ref([]) // 改为存储多个书籍的数组

const handleSearch = async () => {
  if (!searchText.value.trim()) {
    alert('请输入搜索内容')
    return
  }
  
  loading.value = true
  error.value = ''
  searchPerformed.value = true
  foundBooks.value = []
  
  try {
    // 第一步：检查书籍是否存在
    const bookResponse = await getBooks(searchText.value.toLowerCase())
    const books = bookResponse.data || []
    
    if (books.length === 0) {
      return // 没有找到书籍，直接返回
    }
    
    // 第二步：查询书架信息
    const shelfResponse = await getBooksBookShelf(searchText.value.toLowerCase())
    
    // 合并结果
    foundBooks.value = books.map(book => {
      // 查找对应的书架信息
      const shelfInfo = shelfResponse.data?.find(item => item.TITLE === book.Title)
      
      return {
        TITLE: book.Title,
        SHELFID: shelfInfo?.SHELFID || null
      }
    })
    
  } catch (err) {
    console.error('搜索失败:', err)
    error.value = '搜索失败，请稍后重试'
  } finally {
    loading.value = false
  }

}



const location = reactive({
  buildingId: '',
  shelfCode: '',
  floor: '',
  zone: ''
})
</script>

<style scoped>
.search-container {
  max-width: 800px;
  margin: 0 auto;
  padding: 20px;
}

.search-tip {
  color: #666;
  font-size: 14px;
  margin-bottom: 8px;
}

.simple-search {
  display: flex;
  width: 100%;
  max-width: 500px;
  border: 1px solid #ddd;
  border-radius: 4px;
  overflow: hidden;
  margin-bottom: 20px;
}

.simple-search input {
  flex: 1;
  padding: 10px 15px;
  border: none;
  outline: none;
  font-size: 16px;
}

.simple-search button {
  padding: 0 20px;
  border: none;
  background: #1890ff;
  color: white;
  cursor: pointer;
  transition: background 0.3s;
}

.simple-search button:hover {
  background: #40a9ff;
}

.search-icon {
  font-size: 16px;
}

.search-results {
  margin: 30px 0;
  border: 1px solid #eee;
  border-radius: 8px;
  padding: 20px;
  background: white;
}

.loading, .error, .no-results {
  text-align: center;
  padding: 20px;
  color: #666;
}

.error {
  color: #f5222d;
}

.results-title {
  margin-bottom: 15px;
  color: #333;
  font-size: 18px;
}

.books-table {
  width: 100%;
  border-collapse: collapse;
}

.books-table th, .books-table td {
  padding: 12px 15px;
  border-bottom: 1px solid #eee;
  text-align: left;
}

.books-table th {
  background-color: #f7f7f7;
  font-weight: 600;
}

.books-table tr:hover {
  background-color: #f5f5f5;
}

.location-form {
  margin-top: 40px;
  padding: 20px;
  border: 1px solid #eee;
  border-radius: 8px;
  background: white;
}

.form-title {
  margin-bottom: 20px;
  color: #333;
  font-size: 16px;
  font-weight: bold;
}

.form-item {
  margin-bottom: 15px;
  display: flex;
  flex-direction: column;
}

.form-item label {
  margin-bottom: 5px;
  font-size: 14px;
  color: #666;
}

.form-item input {
  padding: 8px 12px;
  border: 1px solid #ddd;
  border-radius: 4px;
  font-size: 14px;
}
</style>