<!-- src/components/LibraryButton.vue -->
<template>
  <button class="my-library-btn" @click="goToAuth">我的图书馆</button>
</template>

<script setup>
import { useRouter } from 'vue-router'

const router = useRouter()

const goToAuth = () => {
  router.push('/auth')
}
</script>

<style scoped>
.my-library-btn {
  padding: 12px 24px;
  background: rgba(0, 0, 255, 0.15);
  border: 1px solid rgba(0, 0, 255, 0.3);
  border-radius: 12px;
  backdrop-filter: blur(8px);
  color: deepskyblue;
  font-size: 20px;
  cursor: pointer;
  transition: all 0.3s ease;
  margin-top: 20px;
}

.my-library-btn:hover {
  background: rgba(0, 0, 255, 0.25);
  transform: scale(1.05);
}
</style>
