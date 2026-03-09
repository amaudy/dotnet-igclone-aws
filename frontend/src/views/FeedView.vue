<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { getPosts } from '../api/posts'
import type { PostResponse } from '../types'
import AppHeader from '../components/AppHeader.vue'
import PostCard from '../components/PostCard.vue'

const posts = ref<PostResponse[]>([])
const loading = ref(true)
const page = ref(1)
const hasMore = ref(false)
const loadingMore = ref(false)
const error = ref('')

async function fetchPosts() {
  try {
    const res = await getPosts(page.value)
    posts.value.push(...res.data.items)
    hasMore.value = page.value < res.data.totalPages
  } catch {
    error.value = 'Failed to load posts'
  }
}

async function loadMore() {
  loadingMore.value = true
  page.value++
  await fetchPosts()
  loadingMore.value = false
}

onMounted(async () => {
  await fetchPosts()
  loading.value = false
})
</script>

<template>
  <AppHeader />
  <main class="pb-20 px-4">
    <div v-if="loading" class="space-y-4 mt-4">
      <div v-for="i in 3" :key="i" class="rounded-xl bg-gray-900 overflow-hidden">
        <div class="flex items-center gap-3 px-4 py-3">
          <div class="h-8 w-8 rounded-full bg-gray-800 animate-pulse"></div>
          <div class="h-4 w-24 rounded bg-gray-800 animate-pulse"></div>
        </div>
        <div class="h-64 w-full bg-gray-800 animate-pulse"></div>
        <div class="px-4 py-3 space-y-2">
          <div class="h-3 w-20 rounded bg-gray-800 animate-pulse"></div>
          <div class="h-3 w-48 rounded bg-gray-800 animate-pulse"></div>
        </div>
      </div>
    </div>

    <div v-else-if="error" class="p-3 rounded-lg bg-red-900/50 text-red-300 text-sm mt-4">
      {{ error }}
    </div>

    <div v-else-if="posts.length === 0" class="text-center py-16 text-gray-500">
      <p class="text-lg">No posts yet</p>
      <p class="text-sm mt-1">Be the first to share something!</p>
    </div>

    <div v-else class="space-y-4">
      <PostCard v-for="post in posts" :key="post.id" :post="post" />

      <div v-if="hasMore" class="flex justify-center py-4">
        <button
          class="px-4 py-1.5 text-sm border border-gray-700 rounded-lg hover:bg-gray-800 disabled:opacity-50"
          :disabled="loadingMore"
          @click="loadMore"
        >
          <span v-if="loadingMore" class="spinner h-4 w-4"></span>
          <span v-else>Load more</span>
        </button>
      </div>
    </div>
  </main>
</template>
