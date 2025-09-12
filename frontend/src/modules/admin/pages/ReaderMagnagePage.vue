<template>
  <div class="reader-management">
    <!-- 搜索栏 -->
    <div class="search-bar">
      <el-input v-model="query.keyword" placeholder="搜索用户名/昵称/真实姓名" clearable @keyup.enter="fetchReaders" />
      <el-button type="primary" @click="fetchReaders">搜索</el-button>
      <el-button type="success" @click="openDialog()">新增读者</el-button>
    </div>

    <!-- 读者表格 -->
    <el-table :data="readers" stripe border style="width: 100%">
      <el-table-column prop="username" label="用户名" width="150" />
      <el-table-column prop="nickname" label="昵称" width="150" />
      <el-table-column prop="realName" label="真实姓名" width="150" />
      <el-table-column prop="credit" label="信誉分" width="100" />
      <el-table-column prop="status" label="账户状态" width="120">
        <template #default="{ row }">
          <el-tag :type="row.status === 'ACTIVE' ? 'success' : 'danger'">
            {{ row.status === 'ACTIVE' ? '正常' : '冻结' }}
          </el-tag>
        </template>
      </el-table-column>
      <el-table-column prop="role" label="权限" width="120" />
      <el-table-column label="操作" width="260">
        <template #default="{ row }">
          <el-button type="primary" size="small" @click="openDialog(row)">编辑</el-button>
          <el-button type="danger" size="small" @click="removeReader(row.id)">删除</el-button>
        </template>
      </el-table-column>
    </el-table>

    <!-- 分页 -->
    <div class="pagination">
      <el-pagination
        background
        layout="prev, pager, next, jumper"
        :total="total"
        v-model:current-page="query.pageNum"
        v-model:page-size="query.pageSize"
        @current-change="fetchReaders"
      />
    </div>

    <!-- 新增/编辑弹窗 -->
    <el-dialog v-model="dialogVisible" :title="form.id ? '编辑读者' : '新增读者'" width="500px">
      <el-form :model="form" label-width="90px">
        <el-form-item label="用户名">
          <el-input v-model="form.username" :disabled="!!form.id" />
        </el-form-item>
        <el-form-item label="昵称">
          <el-input v-model="form.nickname" />
        </el-form-item>
        <el-form-item label="真实姓名">
          <el-input v-model="form.realName" />
        </el-form-item>
        <el-form-item label="信誉分">
          <el-input-number v-model="form.credit" :min="0" />
        </el-form-item>
        <el-form-item label="权限">
          <el-select v-model="form.role" placeholder="请选择权限">
            <el-option label="普通" value="Low" />
            <el-option label="高级" value="High" />
          </el-select>
        </el-form-item>
      </el-form>
      <template #footer>
        <el-button @click="dialogVisible = false">取消</el-button>
        <el-button type="primary" @click="saveReader">确定</el-button>
      </template>
    </el-dialog>
  </div>
</template>

<script setup>
import { ref, reactive, onMounted } from 'vue'
import { ElMessage, ElMessageBox } from 'element-plus'
import { getReaders, getReaderById, addReader, updateReader, deleteReader } from '@/modules/reader/api.js'

const readers = ref([])
const total = ref(0)

const query = reactive({
  keyword: '',
  pageNum: 1,
  pageSize: 10
})

const dialogVisible = ref(false)
const form = reactive({
  id: null,
  username: '',
  nickname: '',
  realName: '',
  credit: 100,
  role: '普通',
  status: 'ACTIVE'
})

const fetchReaders = async () => {
  try {
    const res = await getReaders(query)
    const records = res.data.records || res.data || []  // 看你后端有没有分页包装
    readers.value = records.map(r => ({
      id: r.ReaderID,
      username: r.UserName,
      nickname: r.NickName,
      realName: r.FullName,
      credit: r.CreditScore,
      status: r.AccountStatus === '正常' ? 'ACTIVE' : 'FROZEN',
      role: r.Permission
    }))
    total.value = res.data.total || records.length
    console.log(readers.value)
  } catch (e) {
    console.error(e)
  }
}

// 打开新增/编辑弹窗
const openDialog = async (row) => {
  if (row) {
    const res = await getReaderById(row.id)
    Object.assign(form, res.data)
  } else {
    Object.assign(form, { id: null, username: '', nickname: '', realName: '', credit: 100, role: 'USER', status: 'ACTIVE' })
  }
  dialogVisible.value = true
}

// 保存读者
const saveReader = async () => {
  try {
    if (form.id) {
      await updateReader(form)
      ElMessage.success('编辑成功')
    } else {
      await addReader(form)
      ElMessage.success('新增成功')
    }
    dialogVisible.value = false
    fetchReaders()
  } catch (e) {
    console.error(e)
  }
}

// 删除读者
const removeReader = (id) => {
  ElMessageBox.confirm('确认删除该读者吗？', '提示', { type: 'warning' })
    .then(async () => {
      await deleteReader(id)
      ElMessage.success('删除成功')
      fetchReaders()
    })
    .catch(() => {})
}


onMounted(() => {
  fetchReaders()
})
</script>

<style scoped>
.reader-management {
  padding: 20px;
}
.search-bar {
  margin-bottom: 15px;
  display: flex;
  gap: 10px;
}
.pagination {
  margin-top: 15px;
  text-align: right;
}
</style>
