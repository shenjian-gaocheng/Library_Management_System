<script setup>
import { ref, onMounted } from 'vue';
// 确认 api.js 的路径是否需要调整，这里假设它在 modules/admin/ 下
import { getPublicAnnouncements } from '@/modules/admin/api.js'; 

const announcements = ref([]);
const isLoading = ref(true);

onMounted(async () => {
  try {
    const response = await getPublicAnnouncements();
    announcements.value = response.data;
  } catch (error) {
    console.error("获取公开公告失败:", error);
    // 在生产环境中可以静默失败，不打扰用户
  } finally {
    isLoading.value = false;
  }
});
</script>

<template>
  <div class="carousel-container" v-if="isLoading">
    <p>正在加载公告...</p>
  </div>
  <div class="carousel-container" v-else-if="announcements.length > 0">
    <!-- 
      这里使用了 Element Plus 的 el-carousel 组件作为示例。
      如果您的项目使用其他 UI 库 (如 Ant Design Vue, Naive UI)，
      请替换为对应库的 Carousel/Swiper 组件。
    -->
    <el-carousel height="120px" direction="vertical" :autoplay="true" indicator-position="none" :interval="5000">
      <el-carousel-item v-for="item in announcements" :key="item.announcementID">
        <div class="item-content">
          <h4>{{ item.title }}</h4>
          <p>{{ item.content }}</p>
        </div>
      </el-carousel-item>
    </el-carousel>
  </div>
  <div class="carousel-container" v-else>
    <p>暂无最新公告</p>
  </div>
</template>

<style scoped>
.carousel-container {
  background-color: #ecf5ff;
  border-radius: 4px;
  padding: 20px;
  margin: 20px 0;
  border: 1px solid #b3d8ff;
}
.item-content {
  color: #303133;
  height: 100%;
  display: flex;
  flex-direction: column;
  justify-content: center;
}
.item-content h4 {
  margin: 0 0 8px 0;
  font-size: 1.1rem;
  font-weight: 600;
  white-space: nowrap;
  overflow: hidden;
  text-overflow: ellipsis;
}
.item-content p {
  margin: 0;
  font-size: 0.9rem;
  line-height: 1.6;
  color: #606266;
  /* 多行文本溢出显示省略号 */
  display: -webkit-box;
  -webkit-box-orient: vertical;
  -webkit-line-clamp: 2; /* 限制最多显示2行 */
  overflow: hidden;
}
</style>