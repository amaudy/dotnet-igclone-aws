<script setup lang="ts">
import type { PostResponse } from '../types'

defineProps<{ post: PostResponse }>()

function onImgError(e: Event) {
  const img = e.target as HTMLImageElement
  img.src = 'data:image/svg+xml,<svg xmlns="http://www.w3.org/2000/svg" width="400" height="400" fill="%23333"><rect width="400" height="400"/><text x="50%" y="50%" text-anchor="middle" dy=".3em" fill="%23666" font-size="16">Image unavailable</text></svg>'
}

function timeAgo(dateStr: string): string {
  const seconds = Math.floor((Date.now() - new Date(dateStr).getTime()) / 1000)
  if (seconds < 60) return 'just now'
  const minutes = Math.floor(seconds / 60)
  if (minutes < 60) return `${minutes}m ago`
  const hours = Math.floor(minutes / 60)
  if (hours < 24) return `${hours}h ago`
  const days = Math.floor(hours / 24)
  return `${days}d ago`
}
</script>

<template>
  <div class="rounded-xl bg-gray-900 shadow-md overflow-hidden">
    <div class="flex items-center gap-3 px-4 py-3">
      <div class="w-8 h-8 rounded-full bg-gray-700 text-gray-200 flex items-center justify-center text-sm font-medium">
        {{ post.username.charAt(0).toUpperCase() }}
      </div>
      <RouterLink :to="`/profile/${post.username}`" class="font-semibold text-sm hover:underline">
        {{ post.username }}
      </RouterLink>
    </div>

    <RouterLink :to="`/posts/${post.id}`">
      <img :src="post.imageUrl" :alt="post.caption ?? 'Post image'" class="w-full object-cover" @error="onImgError" />
    </RouterLink>

    <div class="px-4 py-3 space-y-1">
      <div class="flex items-center gap-4 text-sm">
        <span>{{ post.likeCount }} {{ post.likeCount === 1 ? 'like' : 'likes' }}</span>
        <RouterLink :to="`/posts/${post.id}`" class="hover:underline">
          {{ post.commentCount }} {{ post.commentCount === 1 ? 'comment' : 'comments' }}
        </RouterLink>
      </div>
      <p v-if="post.caption" class="text-sm">
        <span class="font-semibold">{{ post.username }}</span>
        {{ post.caption }}
      </p>
      <p class="text-xs text-gray-500">{{ timeAgo(post.createdAt) }}</p>
    </div>
  </div>
</template>
