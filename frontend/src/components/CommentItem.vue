<script setup lang="ts">
import type { CommentResponse } from '../types'

defineProps<{
  comment: CommentResponse
  currentUser: string
}>()

defineEmits<{
  delete: [id: number]
}>()

function timeAgo(dateStr: string): string {
  const seconds = Math.floor((Date.now() - new Date(dateStr).getTime()) / 1000)
  if (seconds < 60) return 'just now'
  const minutes = Math.floor(seconds / 60)
  if (minutes < 60) return `${minutes}m`
  const hours = Math.floor(minutes / 60)
  if (hours < 24) return `${hours}h`
  return `${Math.floor(hours / 24)}d`
}
</script>

<template>
  <div class="flex items-start gap-2 py-2">
    <div class="flex-1 text-sm">
      <RouterLink :to="`/profile/${comment.username}`" class="font-semibold hover:underline">
        {{ comment.username }}
      </RouterLink>
      {{ comment.text }}
      <span class="opacity-50 text-xs ml-1">{{ timeAgo(comment.createdAt) }}</span>
    </div>
    <button
      v-if="comment.username === currentUser"
      class="btn btn-ghost btn-xs text-error"
      @click="$emit('delete', comment.id)"
    >
      <svg xmlns="http://www.w3.org/2000/svg" class="h-4 w-4" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
        <path d="M3 6h18M8 6V4a2 2 0 012-2h4a2 2 0 012 2v2m3 0v14a2 2 0 01-2 2H7a2 2 0 01-2-2V6h14" />
      </svg>
    </button>
  </div>
</template>
