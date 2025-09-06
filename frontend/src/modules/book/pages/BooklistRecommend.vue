<template>
  <div class="p-4">
    <button class="text-blue-500 mb-4" @click="$router.push({ name: 'Booklist' })">
      ⬅ 返回
    </button>
    <h2 class="text-xl font-bold mb-4">猜你喜欢</h2>

    <div v-for="b in store.recommended" :key="b.BooklistId" class="flex justify-between items-center border p-3 mb-2 rounded">
      <div>
        <h3 class="font-semibold">{{ b.BooklistName }}</h3>
        <p class="text-gray-500">{{ b.BooklistIntroduction }}</p>
        <p class="text-sm text-gray-400">创建者: {{ b.CreatorName }}</p>
      </div>
      <button class="px-3 py-1 bg-blue-500 text-white rounded" @click="collect(b.BooklistId)">
        收藏
      </button>
    </div>
  </div>
</template>

<script setup>
import { onMounted } from 'vue'
import { useBooklistStore } from '@/stores/bookliststore'

const store = useBooklistStore()

// 默认随机推荐一个书单ID
onMounted(async () => {
  try {
    // 确保已有书单数据
    if (store.created.length === 0) {
      await store.fetchBooklistsByReader(1, {withToken:true}) // 使用实际用户ID
    }
    
    // 随机选择一个书单ID
    let randomId = 1; // 默认值
    if (store.created.length > 0) {
      const randomIndex = Math.floor(Math.random() * store.created.length)
      randomId = store.created[randomIndex].BooklistId
    }
    
    await store.fetchRecommended(randomId, 10, {withToken:true})
  } catch (error) {
    console.error('获取推荐书单失败:', error)
    // 可以设置一些默认推荐数据，避免页面空白
    store.recommended = []
  }
})

// 收藏书单
async function collect(booklistId) {
  await store.collect(booklistId, { Notes: '' }, {withToken:true})
  alert('收藏成功！')
}
</script>
