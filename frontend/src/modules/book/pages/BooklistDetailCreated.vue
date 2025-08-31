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
      editable-name
      editable-intro
      @edit-name="editName"
      @edit-intro="editIntro"
    />

    <BookItem
      v-for="b in store.currentBooklist?.books"
      :key="b.isbn"
      :book="b"
      show-remove
      @remove="store.removeBook(booklistId, $event)"
    />

    <div class="mt-4">
      <input v-model="isbn" placeholder="输入 ISBN" class="border px-2 py-1 mr-2"/>
      <button class="px-3 py-1 bg-green-500 text-white rounded" @click="addBook">添加图书</button>
    </div>
  </div>
</template>

<script setup>
import { onMounted, ref } from 'vue'
import { useRoute } from 'vue-router'
import { useBooklistStore } from '@/stores/bookliststore'
import BooklistHeader from '../components/BooklistHeader.vue'
import BookItem from '../components/BookItem.vue'

const store = useBooklistStore()
const route = useRoute()
const booklistId = route.params.id
const isbn = ref('')

onMounted(() => {
  store.fetchBooklistDetails(booklistId)
})

function addBook() {
  if (!isbn.value) return
  store.addBook(booklistId, { isbn: isbn.value })
  isbn.value = ''
}

function editName() {
  const newName = prompt('请输入新书单名称')
  if (newName) store.updateName(booklistId, { newName })
}
function editIntro() {
  const newIntro = prompt('请输入新简介')
  if (newIntro) store.updateIntro(booklistId, { newIntro })
}
</script>
