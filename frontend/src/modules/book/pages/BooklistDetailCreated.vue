<template>
  <div class="p-4">
    <button class="text-blue-500 mb-4" @click="$router.push({ name: 'Booklist' })">
      ⬅ 返回
    </button>

    <BooklistHeader
      :name="store.currentBooklist?.BooklistInfo.BooklistName"
      :intro="store.currentBooklist?.BooklistInfo.BooklistIntroduction"
      editable-name
      editable-intro
      @edit-name="editName"
      @edit-intro="editIntro"
    />

    <BookItem
      v-for="b in store.currentBooklist?.Books"
      :key="b.ISBN"
      :book="b"
      show-remove
      @remove="removeBook"
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

// 获取书单详情
onMounted(() => {
  store.fetchBooklistDetails(booklistId, {withToken:true})
})

// 添加图书
async function addBook() {
  if (!isbn.value) return
  await store.addBook(booklistId, { ISBN: isbn.value, Notes: '' }, {withToken:true })
  isbn.value = ''
  store.fetchBooklistDetails(booklistId, {withToken:true})
}

// 移除图书
async function removeBook(isbnValue) {
  await store.removeBook(booklistId, isbnValue)
  store.fetchBooklistDetails(booklistId, {withToken:true})
}

// 修改书单名称
async function editName() {
  const newName = prompt('请输入新书单名称')
  if (newName) {
    await store.updateName(booklistId, { NewName: newName }, {withToken:true})
    store.fetchBooklistDetails(booklistId, {withToken:true})
  }
}

// 修改书单简介
async function editIntro() {
  const newIntro = prompt('请输入新简介')
  if (newIntro) {
    await store.updateIntro(booklistId, { NewIntro: newIntro }, {withToken:true})
    store.fetchBooklistDetails(booklistId, {withToken:true})
  }
}
</script>
