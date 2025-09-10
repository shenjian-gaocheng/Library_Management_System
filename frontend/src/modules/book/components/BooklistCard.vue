<template>
  <div class="flex justify-between items-center border p-3 rounded-lg shadow-sm hover:bg-gray-50">
    <div>
      <h3 class="font-semibold text-lg">{{ booklist.booklistName }}</h3>
      <p class="text-gray-500 text-sm">{{ booklist.booklistIntroduction || booklist.collectNotes }}</p>
    </div>
    <div>
      <button
        v-if="booklist._source === 'created'"
        class="text-red-500 hover:underline"
        @click="$emit('delete', booklist.booklistId)"
      >
        删除
      </button>
      <button
        v-else
        class="text-red-500 hover:underline"
        @click="$emit('cancel-collect', booklist.booklistId)"
      >
        取消收藏
      </button>
    </div>
  </div>
</template>

<script setup>
defineProps({
  booklist: { type: Object, required: true }
})
defineEmits(['delete', 'cancel-collect'])
</script>
