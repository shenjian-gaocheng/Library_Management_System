<template>
  <div class="records-container">
    <!-- 排序方式 -->
    <div class="sort-container" v-if="records.length">
      <label>排序方式：</label>
      <select v-model="sortOrder">
        <option value="desc">借出时间降序</option>
        <option value="asc">借出时间升序</option>
      </select>
    </div>

    <!-- 表格 -->
    <table v-if="records.length" class="styled-table">
      <thead>
        <tr>
          <th>ISBN</th>
          <th>书名</th>
          <th>作者</th>
          <th>借出时间</th>
          <th>归还时间</th>
          <th>逾期罚金</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="(record, index) in paginatedRecords" :key="record.id || index">
          <td>{{ record.ISBN }}</td>
          <td>{{ record.BookTitle }}</td>
          <td>{{ record.BookAuthor }}</td>
          <td>{{ formatDate(record.BorrowTime) }}</td>
          <td>{{ formatDate(record.ReturnTime) }}</td>
          <td>{{ record.OverdueFine }}</td>
        </tr>
      </tbody>
    </table>

    <!-- 没有记录时显示 -->
    <p v-else class="no-data">暂无记录</p>

    <!-- 分页控制 -->
    <div class="pagination" v-if="totalPages >= 1">
      <button @click="prevPage" :disabled="currentPage === 1">上一页</button>
      <span>第 {{ currentPage }} 页 / 共 {{ totalPages }} 页</span>
      <button @click="nextPage" :disabled="currentPage === totalPages">下一页</button>
    </div>
  </div>
</template>

<script setup>
import { ref, computed, watch } from 'vue';

const props = defineProps({
  records: {
    type: Array,
    default: () => []
  },
  pageSize: {
    type: Number,
    default: 10 
  }
});

const currentPage = ref(1);
const sortOrder = ref("desc"); // 默认降序

// 总页数
const totalPages = computed(() => {
  return Math.ceil(props.records.length / props.pageSize);
});

// 当前页数据（根据 BorrowTime 排序）
const paginatedRecords = computed(() => {
  const sorted = [...props.records].sort((a, b) => {
    const timeA = new Date(a.BorrowTime).getTime();
    const timeB = new Date(b.BorrowTime).getTime();
    return sortOrder.value === "desc" ? timeB - timeA : timeA - timeB;
  });
  const start = (currentPage.value - 1) * props.pageSize;
  return sorted.slice(start, start + props.pageSize);
});

// 上一页
const prevPage = () => {
  if (currentPage.value > 1) currentPage.value--;
};

// 下一页
const nextPage = () => {
  if (currentPage.value < totalPages.value) currentPage.value++;
};

// 当 records 改变时重置分页
watch(() => props.records, () => {
  currentPage.value = 1;
});

// 时间格式化函数
const formatDate = (date) => {
  if (!date) return '未归还';
  return new Date(date).toLocaleString();
};
</script>

<style scoped>
.records-container {
  padding: 20px;
  background-color: #f9fbfd;
  border-radius: 10px;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
}

.sort-container {
  margin-bottom: 10px;
  display: flex;
  align-items: center;
  gap: 6px;
  font-size: 14px;
}

.styled-table {
  width: 100%;
  border-collapse: collapse;
  font-size: 14px;
  border-radius: 8px;
  overflow: hidden;
}

.styled-table th {
  background-color: #4da6ff;
  color: white;
  text-align: center;
  padding: 12px;
}

.styled-table td {
  padding: 12px;
  border-bottom: 1px solid #ddd;
  text-align: center;
}

.styled-table tbody tr:nth-child(even) {
  background-color: #f2f8ff;
}

.styled-table tbody tr:hover {
  background-color: #e6f3ff;
}

.no-data {
  text-align: center;
  color: #999;
  padding: 20px;
}

.pagination {
  margin-top: 15px;
  display: flex;
  justify-content: center;
  align-items: center;
  gap: 12px;
}

.pagination button {
  padding: 6px 12px;
  background-color: #4da6ff;
  color: white;
  border: none;
  border-radius: 6px;
  cursor: pointer;
  transition: background 0.2s;
}

.pagination button:hover:not(:disabled) {
  background-color: #3399ff;
}

.pagination button:disabled {
  background-color: #b3d9ff;
  cursor: not-allowed;
}
</style>
