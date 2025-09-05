<template>
  <div class="p-4">
    <h1 class="text-2xl font-bold mb-4">我的书单</h1>

    <!-- 新建书单按钮 -->
    <div class="mb-6">
      <button 
        class="px-4 py-2 bg-green-500 text-white rounded-lg" 
        @click="createNewBooklist"
      >
        ➕ 新建书单
      </button>
    </div>

    <div class="grid grid-cols-2 gap-6">
      <!-- 创建的书单 -->
      <div>
        <h2 class="text-xl font-semibold mb-2">创建书单</h2>
        <BooklistCard
          v-for="b in store.created"
          :key="b.BooklistId"
          :booklist="{ ...b, _source: 'created' }"
          @delete="store.deleteBooklist"
          @click="$router.push({ name: 'BooklistCreated', params: { id: b.BooklistId } })"
          style="cursor: pointer"
        />
      </div>

      <!-- 收藏的书单 -->
      <div>
        <h2 class="text-xl font-semibold mb-2">收藏书单</h2>
        <BooklistCard
          v-for="b in store.collected"
          :key="b.BooklistId"
          :booklist="{ ...b, _source: 'collected' }"
          @cancel-collect="store.cancelCollect"
          @click="$router.push({ name: 'BooklistCollected', params: { id: b.BooklistId } })"
          style="cursor: pointer"
        />
      </div>
    </div>

    <div class="mt-6">
      <button 
        class="px-4 py-2 bg-blue-500 text-white rounded-lg" 
        @click="$router.push({ name: 'BooklistRecommend' })"
      >
        猜你喜欢
      </button>
    </div>
  </div>
</template>

<script setup>
import { onMounted } from 'vue'
import { useBooklistStore } from '@/stores/bookliststore'
import BooklistCard from '../components/BooklistCard.vue'

const store = useBooklistStore()

onMounted(() => {
  store.fetchBooklistsByReader(1, {withToken:true}) // TODO: 改成登录用户的 ReaderId
})

// 新建书单
async function createNewBooklist() {
  const name = prompt('请输入书单名称')
  if (!name) return
  const intro = prompt('请输入书单简介（可选）') || ''

  try {
    await store.createBooklist({
      BooklistName: name,
      BooklistIntroduction: intro
    }, {withToken:true})
    alert('书单创建成功！')
  } catch (err) {
    console.error(err)
    alert('书单创建失败')
  }

  try {
    await store.fetchBooklistsByReader(1, {withToken:true})
  } catch (err) {
    console.error(err)
    alert('更新书单失败')
  }
}
</script>
