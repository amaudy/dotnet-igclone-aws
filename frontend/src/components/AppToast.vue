<script setup lang="ts">
import { useToastStore } from '../stores/toast'

const toast = useToastStore()
</script>

<template>
  <div v-if="toast.visible" class="toast toast-top toast-center z-[100]">
    <div class="alert alert-error">
      <span>{{ toast.message }}</span>
    </div>
  </div>
</template>
