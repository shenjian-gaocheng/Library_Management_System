<template>
  <section class="announcement">
    <h2 class="section-title">最新公告</h2>
    <ul class="announcement-list">
      <li v-for="(item, index) in announcements" :key="index" class="announcement-item">
        <span class="date">{{ item.date }}</span>
        <span class="text">{{ item.text }}</span>
      </li>
    </ul>
  </section>
</template>

<script setup>
const announcements = [
  { date: '2025-07-28', text: '本馆将于8月1日进行系统维护，届时暂停借阅服务。' },
  { date: '2025-07-25', text: '欢迎查阅新上架图书，详见图书资源页。' },
  { date: '2025-07-20', text: '暑期开放时间调整，请关注最新开放安排。' }
]
</script>

<style scoped>
.announcement {
  padding: 2rem 1rem;
  background-color: #ffffff;
  border-top: 1px solid #e1e9f0;
}

.section-title {
  text-align: center;
  font-size: 1.5rem;
  font-weight: bold;
  color: #004b8d;
  margin-bottom: 1.5rem;
}

.announcement-list {
  max-width: 800px;
  margin: auto;
  list-style: none;
  padding: 0;
}

.announcement-item {
  display: flex;
  gap: 1rem;
  padding: 0.75rem 1rem;
  border-bottom: 1px solid #e1e9f0;
  align-items: center;
}

.date {
  flex-shrink: 0;
  color: #666;
  font-size: 0.9rem;
  width: 100px;
}

.text {
  color: #333;
  font-size: 1rem;
  flex: 1;
}
</style>