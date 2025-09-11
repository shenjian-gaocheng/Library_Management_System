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
          <td>{{ record.ISBN || '' }}</td>
          <td>{{ record.BookTitle || '' }}</td>
          <td>{{ record.BookAuthor || '' }}</td>
          <td>{{ record.BorrowTime ? formatDate(record.BorrowTime) : '' }}</td>
          <td :class="{ overdue: getReturnStatus(record) === '已逾期' }">
            {{ getReturnStatus(record) }}
          </td>
          <td>
            {{ record.ISBN ? getOverdueFine(record).toFixed(2) : '' }}
          </td>
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

const fixedRows = 7; // 固定每页显示行数

// 总页数
const totalPages = computed(() => {
  return Math.ceil(props.records.length / props.pageSize);
});

// 当前页数据（排序 + 补空行）
const paginatedRecords = computed(() => {
  const sorted = [...props.records].sort((a, b) => {
    const timeA = new Date(a.BorrowTime).getTime();
    const timeB = new Date(b.BorrowTime).getTime();
    return sortOrder.value === "desc" ? timeB - timeA : timeA - timeB;
  });
  const start = (currentPage.value - 1) * props.pageSize;
  const pageData = sorted.slice(start, start + props.pageSize);

  const filled = [...pageData];
  while (filled.length < fixedRows) {
    filled.push({});
  }
  return filled;
});

// 分页函数
const prevPage = () => {
  if (currentPage.value > 1) currentPage.value--;
};
const nextPage = () => {
  if (currentPage.value < totalPages.value) currentPage.value++;
};

// 当 records 改变时重置分页
watch(() => props.records, () => {
  currentPage.value = 1;
});

// 时间格式化
const formatDate = (date) => {
  if (!date) return '未归还';
  return new Date(date).toLocaleString();
};

// 获取归还状态（超期判断改为 1 天）
const getReturnStatus = (record) => {
  if (!record || !record.ISBN) return '';

  if (record.ReturnTime) {
    return formatDate(record.ReturnTime);
  }

  const borrowTime = new Date(record.BorrowTime).getTime();
  const now = Date.now();
  const borrowDurationDays = (now - borrowTime) / (1000 * 60 * 60 * 24); // 转换为天数

  return borrowDurationDays > 1 ? '已逾期' : '待归还';
};

// 按天计算罚金（与后端算法一致）
const getOverdueFine = (record) => {
  if (!record || !record.BorrowTime) return 0;

  const borrowTime = new Date(record.BorrowTime).getTime();
  const endTime = record.ReturnTime ? new Date(record.ReturnTime).getTime() : Date.now();
  const borrowDurationDays = (endTime - borrowTime) / (1000 * 60 * 60 * 24); // 转换为天数
  const allowedDays = 1; // 允许借1天
  if (borrowDurationDays <= allowedDays) return 0;

  const overdueDays = Math.ceil(borrowDurationDays - allowedDays);
  return overdueDays * 0.1; // 每天 0.1 元
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
  height: 48px;
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

.overdue {
  color: #e57373;
  font-weight: bold;
}
</style>
