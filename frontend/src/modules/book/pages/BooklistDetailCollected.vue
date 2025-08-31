<template>
  <div class="p-4">
    <!-- <button class="text-blue-500 mb-4" @click="$router.back()">⬅ 返回</button> -->
    <button 
    class="text-blue-500 mb-4" 
    @click="$router.push({ name: 'Booklist' })"
    >
    ⬅ 返回
    </button>
    <BooklistHeader
      :name="store.currentBooklist?.booklistInfo.booklistName"
      :intro="store.currentBooklist?.booklistInfo.booklistIntroduction"
      :notes="store.currentBooklist?.collectNotes"
      show-notes
      @edit-notes="editNotes"
    />

    <BookItem
      v-for="b in store.currentBooklist?.books"
      :key="b.isbn"
      :book="b"
    />

    <div class="mt-4">
      <button class="px-3 py-1 bg-red-500 text-white rounded" @click="cancelCollect">
        取消收藏
      </button>
    </div>
  </div>
</template>

<script setup>
import { onMounted } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { useBooklistStore } from '@/stores/bookliststore'
import BooklistHeader from '../components/BooklistHeader.vue'
import BookItem from '../components/BookItem.vue'

const store = useBooklistStore()
const route = useRoute()
const router = useRouter()
const booklistId = route.params.id

onMounted(() => {
  store.fetchBooklistDetails(booklistId, {withToken:true})
})

function editNotes() {
  const newNotes = prompt('请输入新的收藏备注')
  if (newNotes) store.updateCollectNotes(booklistId, { newNotes }, {withToken:true})
}

function cancelCollect() {
  if (confirm('确定要取消收藏吗？')) {
    store.cancelCollect(booklistId, {withToken:true})
    router.push({ name: 'Booklist' })
  }
}
</script>
