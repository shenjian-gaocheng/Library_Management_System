<template>
  <div class="sidebar">
    <div class="logo">我的图书馆</div>
    <ul class="nav-list">
      <li class="nav-item active">首页</li>
      <li class="nav-item">借阅记录</li>
      <li class="nav-item">预约管理</li>
    </ul>
  </div>
</template>

<script setup></script>

<style scoped>
.sidebar {
  width: 220px;
  height: 100vh;
  background: #1e293b;
  color: white;
  padding: 24px 16px;
}
.logo {
  font-size: 20px;
  font-weight: bold;
  margin-bottom: 32px;
}
.nav-list {
  list-style: none;
  padding: 0;
}
.nav-item {
  padding: 12px 8px;
  border-radius: 6px;
  cursor: pointer;
  transition: background 0.3s;
}
.nav-item:hover {
  background: #334155;
}
.nav-item.active {
  background: #475569;
}
</style>
