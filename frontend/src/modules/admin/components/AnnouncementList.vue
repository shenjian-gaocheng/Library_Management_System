<script setup>
// 定义 props，接收父组件传来的数据
defineProps({
  announcements: {
    type: Array,
    default: () => []
  },
  isLoading: {
    type: Boolean,
    default: false
  }
});

// 定义本组件会发出的事件
const emit = defineEmits(['edit-announcement', 'delete-announcement']);

const handleEdit = (announcement) => {
  emit('edit-announcement', announcement);
};

const handleDelete = (id) => {
  if (confirm(`【请确认】您真的要删除ID为 "${id}" 的公告吗？此操作不可撤销。`)) {
    emit('delete-announcement', id);
  }
};
</script>

<template>
  <el-table :data="announcements" v-loading="isLoading" stripe border style="width: 100%">

    <el-table-column prop="announcementID" label="ID" width="80" align="center" />
    
    <el-table-column prop="title" label="公告标题" min-width="200" />
    
    <el-table-column prop="status" label="状态" width="120" align="center">
      <!-- 【净化】使用一个变量来持有 scope.row，避免在模板中多次访问 -->
      <template #default="{ row }">
        <el-tag :type="row.status === '发布中' ? 'success' : 'info'">
          {{ row.status }}
        </el-tag>
      </template>
    </el-table-column>

    <el-table-column prop="targetGroup" label="目标群体" width="120" />
    
    <el-table-column prop="librarianName" label="发布人" width="150" />

    <el-table-column prop="createTime" label="发布时间" width="200">
       <template #default="{ row }">
        {{ new Date(row.createTime).toLocaleString() }}
      </template>
    </el-table-column>
    
    <el-table-column label="操作" fixed="right" width="150" align="center">
      <!-- 【净化】同样使用 row 变量 -->
      <template #default="{ row }">
        <el-button size="small" @click="handleEdit(row)">编辑</el-button>
        <el-button size="small" type="danger" @click="handleDelete(row.announcementID)">删除</el-button>
      </template>
    </el-table-column>
    
    <template #empty>
      <el-empty description="暂无历史公告记录" />
    </template>
    
  </el-table>
</template>