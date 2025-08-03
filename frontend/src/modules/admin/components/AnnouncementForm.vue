<script setup>
import { ref, watch } from 'vue';

// 定义 props，用于接收父组件通过 v-model 传来的数据
const props = defineProps({
  modelValue: {
    type: Object,
    required: true,
  }
});

// 定义 emit，用于向父组件提交事件和更新 v-model
const emit = defineEmits(['submit-announcement', 'update:modelValue']);

// 创建一个本地的、可编辑的表单数据 ref
const formModel = ref({ ...props.modelValue });

// 监听父组件传来的 props 是否变化（当点击“编辑”时会触发）
watch(() => props.modelValue, (newValue) => {
  formModel.value = { ...newValue };
}, { deep: true });

// 提交按钮的逻辑
const handleSubmit = () => {
  if (!formModel.value.title.trim() || !formModel.value.content.trim()) {
    // 使用 Element Plus 的全局消息提示
    ElMessage.error('公告标题和内容不能为空！');
    return;
  }
  // 触发事件，将表单数据提交给父组件
  emit('submit-announcement', { ...formModel.value });
};

// 取消/重置按钮的逻辑
const handleReset = () => {
  // 重置表单为初始的、用于“创建”的状态
  formModel.value = {
      announcementID: null,
      title: '',
      content: '',
      targetGroup: '所有人',
      status: '发布中',
      librarianID: 'A001' // 真实项目中应从用户状态获取
  };
  // 同时通知父组件更新 v-model
  emit('update:modelValue', { ...formModel.value });
}
</script>

<template>
  <el-form :model="formModel" label-position="top">
    <el-form-item label="公告标题" required>
      <el-input v-model="formModel.title" placeholder="请输入公告的标题" />
    </el-form-item>

    <el-form-item label="公告内容" required>
      <el-input
        v-model="formModel.content"
        :rows="5"
        type="textarea"
        placeholder="请输入详细的公告内容"
      />
    </el-form-item>

    <el-row :gutter="20">
      <el-col :span="12">
        <el-form-item label="目标群体">
          <el-select v-model="formModel.targetGroup" style="width: 100%;">
            <el-option label="所有人" value="所有人" />
            <el-option label="读者" value="读者" />
          </el-select>
        </el-form-item>
      </el-col>
      <el-col :span="12">
        <el-form-item label="发布状态">
          <el-select v-model="formModel.status" style="width: 100%;">
            <el-option label="发布中" value="发布中" />
            <el-option label="已撤回" value="已撤回" />
          </el-select>
        </el-form-item>
      </el-col>
    </el-row>

    <el-form-item>
      <div style="width: 100%; display: flex; justify-content: flex-end;">
        <el-button @click="handleReset">重置</el-button>
        <el-button type="primary" @click="handleSubmit">
          {{ modelValue.announcementID ? '确认修改' : '立即发布' }}
        </el-button>
      </div>
    </el-form-item>
  </el-form>
</template>