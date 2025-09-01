<script setup>
import { ref, onMounted, computed } from 'vue';
// 导入 Element Plus 的消息弹窗组件
import { ElMessageBox } from 'element-plus';
// 导入我们的 API 函数
import { getPublicAnnouncements } from '@/modules/admin/api.js';

// --- 状态变量 ---
const realAnnouncements = ref([]); // 存放从后端获取的原始公告数据
const isLoading = ref(true);

// --- 生命周期钩子 ---
// 在组件挂载到页面时，自动获取数据
onMounted(async () => {
  try {
    const response = await getPublicAnnouncements();
    realAnnouncements.value = response.data;
  } catch (error) {
    console.error("获取首页公告失败:", error);
    // 即使获取失败，也要结束加载状态，以显示占位符
  } finally {
    isLoading.value = false;
  }
});

// --- 计算属性 ---
// 这是组件的核心逻辑：动态地、响应式地生成最终要显示的3条数据
const displayAnnouncements = computed(() => {
  // 1. 先将后端返回的真实数据，格式化为我们需要的显示格式
  const formatted = realAnnouncements.value.map(item => ({
    id: item.announcementID,
    // 将日期格式化为 YYYY-MM-DD
    date: new Date(item.createTime).toISOString().split('T')[0],
    title: item.title,
    content: item.content,
    isPlaceholder: false // 标记为真实数据
  }));

  // 2. 如果格式化后的真实数据不足3条，用占位符补足，直到满3条为止
  while (formatted.length < 3) {
    formatted.push({
      id: `placeholder-${formatted.length}`,
      date: '---- / -- / --',
      title: '暂无更多公告',
      content: '',
      isPlaceholder: true // 标记为占位符
    });
  }
  
  // 3. 返回这个永远包含3个元素的数组
  return formatted;
});

// --- 事件处理器 ---
// 当用户点击某条公告标题时触发
const showDetail = (announcement) => {
  // 如果点击的不是占位符，才执行弹窗逻辑
  if (!announcement.isPlaceholder) {
    // 使用 Element Plus 的 ElMessageBox 来显示一个更简洁、更漂亮的弹窗
    ElMessageBox.alert(
      // 使用 <pre> 标签来保留公告内容中的换行和空格格式
      `<pre class="dialog-content-pre">${announcement.content}</pre>`, 
      announcement.title, // 弹窗标题
      {
        dangerouslyUseHTMLString: true, // 必须设置此项，才能让 <pre> 标签生效
        confirmButtonText: '关闭',
        // 可以通过 customClass 添加自定义样式
        customClass: 'announcement-dialog' 
      }
    )
  }
}
</script>

<template>
  <section class="announcement-section">
    <h2 class="section-title">最新公告</h2>
    
    <div v-if="isLoading" class="loading-state">
      正在加载最新公告...
    </div>

    <!-- 当加载完成时，显示列表 -->
    <ul v-else class="announcement-list">
      <li 
        v-for="item in displayAnnouncements" 
        :key="item.id" 
        class="announcement-item"
        :class="{ 'is-placeholder': item.isPlaceholder }"
        @click="showDetail(item)"
      >
        <span class="date">{{ item.date }}</span>
        <!-- 不再使用 <a> 标签，因为整个 <li> 都可以点击 -->
        <span class="title">{{ item.title }}</span>
      </li>
    </ul>
  </section>
</template>

<style>
/* 
  【重要】
  因为 ElMessageBox 创建的弹窗是在 <body> 的根节点，
  不受 <style scoped> 的影响，所以我们需要把它的自定义样式放在全局
  或者一个非 scoped 的 <style> 标签里。
*/
.announcement-dialog .el-message-box__content {
  /* 允许弹窗内容滚动 */
  max-height: 60vh;
  overflow-y: auto;
}

.announcement-dialog .dialog-content-pre {
  white-space: pre-wrap; /* 自动换行 */
  word-wrap: break-word; /* 单词内换行 */
  font-family: inherit; /* 使用页面默认字体 */
  font-size: 1rem;
  line-height: 1.8;
  margin: 0;
  color: #606266;
}
</style>

<style scoped>
.announcement-section {
  padding: 3rem 1rem;
  background-color: #f8f9fa;
  border-top: 1px solid #e1e9f0;
}
.section-title {
  text-align: center;
  font-size: 1.8rem;
  font-weight: 300;
  color: #004b8d;
  margin-bottom: 2rem;
  letter-spacing: .1em;
}
.announcement-list {
  max-width: 800px;
  margin: 0 auto;
  list-style: none;
  padding: 0;
  border: 1px solid #e1e9f0;
  border-radius: 8px;
  overflow: hidden;
  box-shadow: 0 4px 12px rgba(0,0,0,0.05);
  background-color: #ffffff;
}
.announcement-item {
  display: flex;
  gap: 1.5rem;
  padding: 1rem 1.5rem;
  border-bottom: 1px solid #e1e9f0;
  align-items: center;
  transition: background-color 0.2s ease;
}
.announcement-item:last-child {
  border-bottom: none;
}

/* 对占位符和可点击项应用不同样式 */
.announcement-item:not(.is-placeholder) {
  cursor: pointer;
}
.announcement-item:not(.is-placeholder):hover {
  background-color: #f1f3f5;
}

.date {
  flex-shrink: 0;
  color: #868e96;
  font-size: 0.9rem;
  width: 100px;
  font-family: monospace;
}
.title {
  color: #343a40;
  font-size: 1rem;
  font-weight: 500;
  flex: 1;
  transition: color 0.2s ease;
}
.announcement-item:not(.is-placeholder):hover .title {
  color: #0056b3;
}
.announcement-item.is-placeholder .title {
  color: #adb5bd; /* 让占位符文本颜色变浅 */
}

.loading-state {
  text-align: center;
  color: #888;
  padding: 2rem;
}
</style>