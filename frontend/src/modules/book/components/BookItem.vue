<template>
  <div class="flex justify-between items-center border-b py-2">
    <div>
      <p class="font-medium">{{ book.title }} (ISBN: {{ book.isbn }})</p>
      <p class="text-gray-500 text-sm">作者: {{ book.author }} | 添加时间: {{ book.addedAt }}</p>
    </div>
    <div v-if="showRemove">
      <button class="text-red-500 hover:underline" @click="$emit('remove', book.isbn)">删除</button>
    </div>
  </div>
</template>

<script setup>
defineProps({
  book: { type: Object, required: true },
  showRemove: { type: Boolean, default: false }
})
defineEmits(['remove'])
</script>
