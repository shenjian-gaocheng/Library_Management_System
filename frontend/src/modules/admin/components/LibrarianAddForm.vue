<script setup>
import { ref } from 'vue';

const newLibrarian = ref({
  librarianID: '',
  name: '',
  password: '',
  permission: '普通'
});

const emit = defineEmits(['add-librarian']);

const handleSubmit = () => {
  const { librarianID, name, password } = newLibrarian.value;
  if (!librarianID || !name || !password) {
    alert('管理员ID、姓名和密码不能为空！');
    return;
  }
  // 发出一个 'add-librarian' 事件，把整个表单数据对象传出去
  emit('add-librarian', { ...newLibrarian.value });
  // 清空表单
  newLibrarian.value = { librarianID: '', name: '', password: '', permission: '普通' };
};
</script>

<template>
  <div class="add-form">
    <h3>新增管理员</h3>
    <input v-model="newLibrarian.librarianID" placeholder="管理员ID (例如: A001)" />
    <input v-model="newLibrarian.name" placeholder="姓名" />
    <input v-model="newLibrarian.password" type="password" placeholder="初始密码" />
    <select v-model="newLibrarian.permission">
      <option value="普通">普通</option>
      <option value="高级">高级</option>
    </select>
    <button @click="handleSubmit">确认新增</button>
  </div>
</template>

<style scoped>
.add-form {
  border: none;
  padding: 0;
  margin: 0 0 1.5rem 0; /* 与查询表单拉开距离 */
  background: none;
}
input, select, button {
  padding: 8px;
  border: 1px solid #ccc;
  border-radius: 4px;
}
</style>