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
  <div class="card bg-base-200 shadow-md">
    <div class="flex items-center gap-3 px-4 py-3">
      <div class="avatar placeholder">
        <div class="bg-neutral text-neutral-content w-8 h-8 rounded-full flex items-center justify-center">
          <span class="text-sm">{{ post.username.charAt(0).toUpperCase() }}</span>
        </div>
      </div>
      <RouterLink :to="`/profile/${post.username}`" class="font-semibold text-sm hover:underline">
        {{ post.username }}
      </RouterLink>
    </div>

    <RouterLink :to="`/posts/${post.id}`">
      <figure>
        <img :src="post.imageUrl" :alt="post.caption ?? 'Post image'" class="w-full object-cover" @error="onImgError" />
      </figure>
    </RouterLink>

    <div class="card-body px-4 py-3 gap-1">
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
      <p class="text-xs opacity-50">{{ timeAgo(post.createdAt) }}</p>
    </div>
  </div>
</template>
