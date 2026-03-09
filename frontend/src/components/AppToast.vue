<script setup lang="ts">
import { useToastStore } from '../stores/toast'

const toast = useToastStore()
</script>

<template>
  <div v-if="toast.visible" class="fixed top-4 left-1/2 -translate-x-1/2 z-[100]">
    <div class="px-4 py-3 rounded-lg bg-red-900/80 text-red-200 shadow-lg">
      {{ toast.message }}
    </div>
  </div>
</template>
