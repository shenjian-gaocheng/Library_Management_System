<template>
  <p class="search-tip">请输入要查询或加入书架的书籍</p>
  <div class="simple-search">
    
    <input
      v-model="searchText"
      type="text"
      placeholder="搜索图书..."
      @input="handleSearch"
    />
    <button @click="handleSearch">
      <i class="search-icon">搜索</i>
    </button>
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
import { ref } from 'vue'
import { reactive } from 'vue'
const searchText = ref('')

const handleSearch = () => {
  console.log('当前搜索词:', searchText.value)
  // 这里可以触发搜索逻辑
}

const location = reactive({
  buildingId: '',
  shelfCode: '',
  floor: '',
  zone: ''
})
</script>

<style scoped>

.search-tip {
  color: #666;
  font-size: 14px;
  margin-bottom: 8px;
}
.simple-search {
  display: flex;
  width: 300px;
  border: 1px solid #ddd;
  border-radius: 4px;
  overflow: hidden;
}

.simple-search input {
  flex: 1;
  padding: 8px 12px;
  border: none;
  outline: none;
}

.simple-search button {
  padding: 0 12px;
  border: none;
  background: #f5f5f5;
  cursor: pointer;
}

.search-icon {
  font-size: 16px;
}

.location-form {
  max-width: 500px;
  margin: 20px 0;
  padding: 15px;
  border: 1px solid #eee;
  border-radius: 8px;
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