<script setup>
import { ref, onMounted } from 'vue';
import { 
  getAllAnnouncementsForManagement, 
  createAnnouncement, 
  updateAnnouncement, 
  deleteAnnouncement 
} from '@/modules/admin/api.js';
import AnnouncementList from '../components/AnnouncementList.vue';
import AnnouncementForm from '../components/AnnouncementForm.vue';

// 状态变量
const announcements = ref([]);
const isLoading = ref(true);

// 用于 v-model 绑定到表单的数据对象
const currentAnnouncement = ref({
  announcementID: null,
  title: '',
  content: '',
  targetGroup: '所有人',
  status: '发布中',
  librarianID: 'A001' // 真实项目中应从用户状态获取
});

// 获取列表数据
const fetchAnnouncements = async () => {
  isLoading.value = true;
  try {
    const response = await getAllAnnouncementsForManagement();
    announcements.value = response.data;
  } catch (error) {
    ElMessage.error('获取公告列表失败，请检查网络或联系管理员。');
    console.error("获取公告列表失败:", error);
  } finally {
    isLoading.value = false;
  }
};

// 组件挂载时，自动获取一次数据
onMounted(fetchAnnouncements);

// 处理表单的提交事件（可能是新建或更新）
const handleSave = async (formData) => {
  try {
    if (formData.announcementID) {
      // 有 ID，是更新操作
      await updateAnnouncement(formData.announcementID, formData);
      ElMessage.success('公告修改成功！');
    } else {
      // 没有 ID，是创建操作
      await createAnnouncement(formData);
      ElMessage.success('新公告发布成功！');
    }
    // 操作成功后，重置表单并刷新列表
    currentAnnouncement.value = { announcementID: null, title: '', content: '', targetGroup: '所有人', status: '发布中', librarianID: 'A001' };
    await fetchAnnouncements();
  } catch (error) {
    ElMessage.error('操作失败，请重试。');
    console.error(error);
  }
};

// 处理列表传来的“编辑”事件
const handleEdit = (announcement) => {
  // 将要编辑的数据，填充到与表单绑定的 ref 中
  currentAnnouncement.value = { ...announcement }; 
  // 滚动到页面顶部，让用户能看到已填充的表单
  window.scrollTo({ top: 0, behavior: 'smooth' });
};

// 处理列表传来的“删除”事件
const handleDelete = async (id) => {
  try {
    await deleteAnnouncement(id);
    ElMessage.success(`ID为 "${id}" 的公告已成功删除。`);
    await fetchAnnouncements(); // 刷新列表
  } catch (error) {
    ElMessage.error('删除失败，请重试。');
    console.error("删除公告失败:", error);
  }
};
</script>

<template>
  <div class="page-container">
    <h1 class="page-title">公告发布与管理</h1>
    
    <el-card class="box-card" :header="currentAnnouncement.announcementID ? `编辑公告 (ID: ${currentAnnouncement.announcementID})` : '创建新公告'">
      <AnnouncementForm 
        v-model="currentAnnouncement" 
        @submit-announcement="handleSave" 
      />
    </el-card>

    <el-card class="box-card" header="历史公告列表" style="margin-top: 2rem;">
      <AnnouncementList 
        :announcements="announcements" 
        :is-loading="isLoading"
        @edit-announcement="handleEdit"
        @delete-announcement="handleDelete"
      />
    </el-card>
  </div>
</template>

<style scoped>
.page-container {
  padding: 20px 40px;
  background-color: #f7f8fa;
  min-height: calc(100vh - 100px); /* 确保内容区足够高 */
}
.page-title {
  font-size: 2rem;
  font-weight: 400;
  color: #303133;
  margin-bottom: 2rem;
}
.box-card {
  width: 100%;
}
</style>