<template>
  <div class="p-4">
    <!-- <button class="text-blue-500 mb-4" @click="$router.back()">⬅ 返回</button> -->
    <button 
    class="text-blue-500 mb-4" 
    @click="$router.push({ name: 'Booklist' })"
    >
    ⬅ 返回
    </button>
    <h2 class="text-xl font-bold mb-4">猜你喜欢</h2>

    <div v-for="b in store.recommended" :key="b.booklistId" class="flex justify-between items-center border p-3 mb-2 rounded">
      <div>
        <h3 class="font-semibold">{{ b.booklistName }}</h3>
        <p class="text-gray-500">{{ b.booklistIntroduction }}</p>
        <p class="text-sm text-gray-400">创建者: {{ b.creatorName }}</p>
      </div>
      <button class="px-3 py-1 bg-blue-500 text-white rounded" @click="collect(b.booklistId)">
        收藏
      </button>
    </div>
  </div>
</template>

<script setup>
import { onMounted } from 'vue'
import { useBooklistStore } from '@/stores/bookliststore'

const store = useBooklistStore()

onMounted(() => {
  store.fetchRecommended(1, {withToken:true}) // TODO: 默认传入一个书单Id，也可以改成随机取 created 中的一个
})

function collect(booklistId) {
  store.collect(booklistId, { notes: '' }, {withToken:true})
}
</script>
