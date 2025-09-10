<template>
  <div class="p-4">
    <h1 class="text-2xl font-bold mb-4">我的书单</h1>

    <div class="grid grid-cols-2 gap-6">
      <!-- 创建的书单 -->
      <div>
        <h2 class="text-xl font-semibold mb-2">创建书单</h2>
        <BooklistCard
          v-for="b in store.created"
          :key="b.booklistId"
          :booklist="{ ...b, _source: 'created' }"
          @delete="store.deleteBooklist"
          @click="$router.push({ name: 'BooklistCreated', params: { id: b.booklistId } })"
        />
      </div>

      <!-- 收藏的书单 -->
      <div>
        <h2 class="text-xl font-semibold mb-2">收藏书单</h2>
        <BooklistCard
          v-for="b in store.collected"
          :key="b.booklistId"
          :booklist="{ ...b, _source: 'collected' }"
          @cancel-collect="store.cancelCollect"
          @click="$router.push({ name: 'BooklistCollected', params: { id: b.booklistId } })"
        />
      </div>
    </div>

    <div class="mt-6">
      <button class="px-4 py-2 bg-blue-500 text-white rounded-lg" @click="$router.push({ name: 'BooklistRecommend' })">
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
  store.fetchBooklistsByReader(1, {withToken:true}) // TODO: 改成登录用户的 readerId
})
</script>
