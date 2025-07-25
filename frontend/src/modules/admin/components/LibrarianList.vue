<script setup>
// 这个组件非常“纯粹”，它只接收数据并展示
// 当需要删除时，它不自己调用API，而是通知父组件（页面）去处理
defineProps({
  librarians: {
    type: Array,
    required: true
  }
});

const emit = defineEmits(['delete-librarian']);

const handleDelete = (id) => {
  if (!confirm(`确定要删除ID为 ${id} 的管理员吗？`)) {
    return;
  }
  // 发出一个名为 'delete-librarian' 的事件，并把要删除的id传出去
  emit('delete-librarian', id);
};
</script>

<template>
  <div class="list-container">
    <h3>管理员列表</h3>
    <table>
      <thead>
        <tr>
          <th>管理员ID</th>
          <th>姓名</th>
          <th>权限等级</th>
          <th>操作</th>
        </tr>
      </thead>
      <tbody>
        <tr v-if="librarians.length === 0">
          <td colspan="4">暂无数据</td>
        </tr>
        <tr v-for="lib in librarians" :key="lib.librarianID">
          <td>{{ lib.librarianID }}</td>
          <td>{{ lib.name }}</td>
          <td>{{ lib.permission }}</td>
          <td>
            <button @click="handleDelete(lib.librarianID)" class="delete-btn">删除</button>
          </td>
        </tr>
      </tbody>
    </table>
  </div>
</template>

<style scoped>
td {
  border: 1px solid #ddd;
  padding: 12px;
  text-align: left;
}
th {
  background-color: #f2f2f2;
}
.delete-btn {
  background-color: #f44336;
  color: white;
  border: none;
  cursor: pointer;
  padding: 5px 10px;
  border-radius: 4px;
}
.list-container {
  background: #ffffff;
  padding: 1.5rem;
  border-radius: 8px;
  box-shadow: 0 2px 4px rgba(0,0,0,0.05);
}
.list-container h3 {
  font-size: 1.2rem;
  color: #495057;
  padding-bottom: 0.5rem;
  border-bottom: 1px solid #e9ecef;
  margin: 0 0 1rem 0;
}
table {
  border: none; /* 移除外边框 */
}
th {
  background-color: #f8f9fa;
  color: #495057;
  text-transform: uppercase;
  font-size: 0.85rem;
  letter-spacing: 0.05em;
}
tr:nth-child(even) {
  background-color: #f8f9fa; /* 斑马条纹 */
}
tr:hover {
  background-color: #e9ecef; /* 悬浮高亮 */
}
</style>