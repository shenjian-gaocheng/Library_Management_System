<template>
  <div class="container">
    <h1>借阅记录管理系统</h1>
    
    <!-- API基础URL设置 -->
    <div class="form-group">
      <label for="baseUrl">API基础URL:</label>
      <input 
        type="text" 
        id="baseUrl" 
        v-model="baseUrl" 
        placeholder="例如：http://localhost:5000/api"
      />
    </div>
    
    <!-- 功能选项卡 -->
    <div class="tabs">
      <button 
        @click="activeTab = 'getAll'" 
        :class="{ active: activeTab === 'getAll' }"
      >
        查询所有记录
      </button>
      <button 
        @click="activeTab = 'getById'" 
        :class="{ active: activeTab === 'getById' }"
      >
        按ID查询
      </button>
      <button 
        @click="activeTab = 'getByReader'" 
        :class="{ active: activeTab === 'getByReader' }"
      >
        按读者ID查询
      </button>
      <button 
        @click="activeTab = 'getByBook'" 
        :class="{ active: activeTab === 'getByBook' }"
      >
        按图书ID查询
      </button>
      <button 
        @click="activeTab = 'add'" 
        :class="{ active: activeTab === 'add' }"
      >
        添加记录
      </button>
      <button 
        @click="activeTab = 'update'" 
        :class="{ active: activeTab === 'update' }"
      >
        更新记录
      </button>
      <button 
        @click="activeTab = 'delete'" 
        :class="{ active: activeTab === 'delete' }"
      >
        删除记录
      </button>
    </div>
    
    <!-- 功能内容区域 -->
    <div class="tab-content">
      <!-- 查询所有借阅记录 -->
      <div v-show="activeTab === 'getAll'" class="tab-pane">
        <button @click="getAllBorrowRecords" class="btn-primary">
          查询所有记录
        </button>
      </div>
      
      <!-- 按ID查询借阅记录 -->
      <div v-show="activeTab === 'getById'" class="tab-pane">
        <div class="form-group">
          <label for="queryById">借阅记录ID:</label>
          <input 
            type="number" 
            id="queryById" 
            v-model="queryById" 
            placeholder="请输入借阅记录ID"
          />
        </div>
        <button @click="getBorrowRecordById" class="btn-primary">
          查询
        </button>
      </div>
      
      <!-- 按读者ID查询 -->
      <div v-show="activeTab === 'getByReader'" class="tab-pane">
        <div class="form-group">
          <label for="queryByReader">读者ID:</label>
          <input 
            type="text" 
            id="queryByReader" 
            v-model="queryByReader" 
            placeholder="请输入读者ID"
          />
        </div>
        <button @click="getBorrowRecordsByReader" class="btn-primary">
          查询
        </button>
      </div>
      
      <!-- 按图书ID查询 -->
      <div v-show="activeTab === 'getByBook'" class="tab-pane">
        <div class="form-group">
          <label for="queryByBook">图书ID:</label>
          <input 
            type="text" 
            id="queryByBook" 
            v-model="queryByBook" 
            placeholder="请输入图书ID"
          />
        </div>
        <button @click="getBorrowRecordsByBook" class="btn-primary">
          查询
        </button>
      </div>
      
      <!-- 添加借阅记录 -->
      <div v-show="activeTab === 'add'" class="tab-pane">
        <div class="form-group">
          <label for="addBookId">图书ID:</label>
          <input 
            type="text" 
            id="addBookId" 
            v-model="newRecord.bookId" 
            placeholder="请输入图书ID"
          />
        </div>
        <div class="form-group">
          <label for="addReaderId">读者ID:</label>
          <input 
            type="text" 
            id="addReaderId" 
            v-model="newRecord.readerId" 
            placeholder="请输入读者ID"
          />
        </div>
        <div class="form-group">
          <label for="addBorrowTime">借阅时间:</label>
          <input 
            type="datetime-local" 
            id="addBorrowTime" 
            v-model="newRecord.borrowTime"
          />
        </div>
        <div class="form-group">
          <label for="addReturnTime">归还时间:</label>
          <input 
            type="datetime-local" 
            id="addReturnTime" 
            v-model="newRecord.returnTime"
          />
        </div>
        <div class="form-group">
          <label for="addOverdueFine">逾期费用:</label>
          <input 
            type="number" 
            id="addOverdueFine" 
            v-model="newRecord.overdueFine" 
            placeholder="请输入逾期费用"
          />
        </div>
        <button @click="addBorrowRecord" class="btn-primary">
          添加记录
        </button>
      </div>
      
      <!-- 更新借阅记录 -->
      <div v-show="activeTab === 'update'" class="tab-pane">
        <div class="form-group">
          <label for="updateId">记录ID:</label>
          <input 
            type="number" 
            id="updateId" 
            v-model="updateRecord.borrowRecordId" 
            placeholder="请输入要更新的记录ID"
          />
        </div>
        <div class="form-group">
          <label for="updateBookId">图书ID:</label>
          <input 
            type="text" 
            id="updateBookId" 
            v-model="updateRecord.bookId" 
            placeholder="请输入图书ID"
          />
        </div>
        <div class="form-group">
          <label for="updateReaderId">读者ID:</label>
          <input 
            type="text" 
            id="updateReaderId" 
            v-model="updateRecord.readerId" 
            placeholder="请输入读者ID"
          />
        </div>
        <div class="form-group">
          <label for="updateBorrowTime">借阅时间:</label>
          <input 
            type="datetime-local" 
            id="updateBorrowTime" 
            v-model="updateRecord.borrowTime"
          />
        </div>
        <div class="form-group">
          <label for="updateReturnTime">归还时间:</label>
          <input 
            type="datetime-local" 
            id="updateReturnTime" 
            v-model="updateRecord.returnTime"
          />
        </div>
        <div class="form-group">
          <label for="updateOverdueFine">逾期费用:</label>
          <input 
            type="number" 
            id="updateOverdueFine" 
            v-model="updateRecord.overdueFine" 
            placeholder="请输入逾期费用"
          />
        </div>
        <button @click="updateBorrowRecord" class="btn-primary">
          更新记录
        </button>
      </div>
      
      <!-- 删除借阅记录 -->
      <div v-show="activeTab === 'delete'" class="tab-pane">
        <div class="form-group">
          <label for="deleteId">记录ID:</label>
          <input 
            type="number" 
            id="deleteId" 
            v-model="deleteId" 
            placeholder="请输入要删除的记录ID"
          />
        </div>
        <button @click="deleteBorrowRecord" class="btn-danger">
          删除记录
        </button>
      </div>
    </div>
    
    <!-- 结果展示区域 -->
    <div class="result-container">
      <h3>响应结果:</h3>
      
      <div class="status-info">
        <p v-if="responseStatus">
          状态码: <span class="status-code">{{ responseStatus.code }}</span>
          | 状态文本: {{ responseStatus.text }}
          | 请求时间: {{ responseStatus.time }}
        </p>
        <p v-else>暂无请求</p>
      </div>
      
      <div class="response-data">
        <pre v-if="responseData">{{ formattedResponse }}</pre>
        <div v-else>暂无数据</div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, reactive, toRaw } from 'vue';
import axios from 'axios';

// 基础URL
const baseUrl = ref('http://localhost:5000/api');

// 激活的标签页
const activeTab = ref('getAll');

// 查询参数
const queryById = ref('');
const queryByReader = ref('');
const queryByBook = ref('');
const deleteId = ref('');

// 新增记录数据
const newRecord = reactive({
  bookId: '',
  readerId: '',
  borrowTime: new Date().toISOString().slice(0, 16),
  returnTime: null,
  overdueFine: 0
});

// 更新记录数据
const updateRecord = reactive({
  borrowRecordId: '',
  bookId: '',
  readerId: '',
  borrowTime: null,
  returnTime: null,
  overdueFine: 0
});

// 响应结果
const responseData = ref(null);
const responseStatus = ref(null);

// 格式化响应数据用于展示
const formattedResponse = ref('');

// 通用请求函数
const apiRequest = async (method, url, data = null) => {
  try {
    const startTime = new Date();
    const fullUrl = `${baseUrl.value}/borrowing${url}`;
    
    let response;
    switch(method) {
      case 'get':
        response = await axios.get(fullUrl);
        break;
      case 'post':
        response = await axios.post(fullUrl, data);
        break;
      case 'put':
        response = await axios.put(fullUrl, data);
        break;
      case 'delete':
        response = await axios.delete(fullUrl);
        break;
      default:
        throw new Error('不支持的请求方法');
    }
    
    const endTime = new Date();
    
    // 更新响应状态
    responseStatus.value = {
      code: response.status,
      text: response.statusText,
      time: `${endTime - startTime}ms`
    };
    
    // 更新响应数据
    responseData.value = response.data;
    formattedResponse.value = JSON.stringify(response.data, null, 2);
    
    return response.data;
  } catch (error) {
    const endTime = new Date();
    responseStatus.value = {
      code: error.response?.status || '未知',
      text: error.response?.statusText || error.message,
      time: 'N/A'
    };
    
    responseData.value = error.response?.data || { error: error.message };
    formattedResponse.value = JSON.stringify(responseData.value, null, 2);
    
    throw error;
  }
};

// 获取所有借阅记录
const getAllBorrowRecords = async () => {
  await apiRequest('get', '');
};

// 按ID查询借阅记录
const getBorrowRecordById = async () => {
  if (!queryById.value) {
    alert('请输入借阅记录ID');
    return;
  }
  await apiRequest('get', `/${queryById.value}`);
};

// 按读者ID查询
const getBorrowRecordsByReader = async () => {
  if (!queryByReader.value) {
    alert('请输入读者ID');
    return;
  }
  await apiRequest('get', `/reader/${queryByReader.value}`);
};

// 按图书ID查询
const getBorrowRecordsByBook = async () => {
  if (!queryByBook.value) {
    alert('请输入图书ID');
    return;
  }
  await apiRequest('get', `/book/${queryByBook.value}`);
};

// 添加借阅记录
const addBorrowRecord = async () => {
  if (!newRecord.bookId || !newRecord.readerId || !newRecord.borrowTime) {
    alert('图书ID、读者ID和借阅时间为必填项');
    return;
  }
  
  await apiRequest('post', '', toRaw(newRecord));
};

// 更新借阅记录
const updateBorrowRecord = async () => {
  if (!updateRecord.borrowRecordId || !updateRecord.bookId || !updateRecord.readerId) {
    alert('记录ID、图书ID和读者ID为必填项');
    return;
  }
  
  await apiRequest('put', '', toRaw(updateRecord));
};

// 删除借阅记录
const deleteBorrowRecord = async () => {
  if (!deleteId.value) {
    alert('请输入要删除的借阅记录ID');
    return;
  }
  
  if (confirm(`确定要删除ID为${deleteId.value}的借阅记录吗？`)) {
    await apiRequest('delete', `/${deleteId.value}`);
  }
};
</script>

<style scoped>
.container {
  max-width: 800px;
  margin: 0 auto;
  padding: 20px;
  font-family: Arial, sans-serif;
}

h1 {
  text-align: center;
  color: #333;
}

.form-group {
  margin-bottom: 15px;
}

label {
  display: block;
  margin-bottom: 5px;
  font-weight: bold;
}

input {
  width: 100%;
  padding: 8px;
  border: 1px solid #ccc;
  border-radius: 4px;
  box-sizing: border-box;
}

.tabs {
  display: flex;
  flex-wrap: wrap;
  margin-bottom: 20px;
  border-bottom: 1px solid #ccc;
}

.tabs button {
  padding: 10px 15px;
  margin-right: 5px;
  background-color: #f1f1f1;
  border: none;
  cursor: pointer;
  transition: background-color 0.3s;
}

.tabs button.active {
  background-color: #4CAF50;
  color: white;
}

.tab-content .tab-pane {
  padding: 15px;
  border: 1px solid #ccc;
  border-top: none;
  margin-bottom: 20px;
}

button {
  padding: 8px 15px;
  border: none;
  border-radius: 4px;
  cursor: pointer;
  color: white;
}

.btn-primary {
  background-color: #4CAF50;
}

.btn-danger {
  background-color: #f44336;
}

.result-container {
  margin-top: 20px;
  padding: 15px;
  background-color: #f9f9f9;
  border-radius: 4px;
}

.status-info {
  margin-bottom: 10px;
  padding: 10px;
  background-color: #e8f5e9;
  border-radius: 4px;
}

.status-code {
  font-weight: bold;
  color: #4CAF50;
}

.response-data pre {
  background-color: #fff;
  padding: 10px;
  border-radius: 4px;
  border: 1px solid #ddd;
  overflow-x: auto;
}
</style>