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
  <main class="max-w-lg mx-auto pt-16 pb-20 px-4">
    <div v-if="loading" class="space-y-4 mt-4">
      <div v-for="i in 3" :key="i" class="card bg-base-200">
        <div class="flex items-center gap-3 px-4 py-3">
          <div class="skeleton h-8 w-8 rounded-full"></div>
          <div class="skeleton h-4 w-24"></div>
        </div>
        <div class="skeleton h-64 w-full"></div>
        <div class="px-4 py-3 space-y-2">
          <div class="skeleton h-3 w-20"></div>
          <div class="skeleton h-3 w-48"></div>
        </div>
      </div>
    </div>

    <div v-else-if="error" class="alert alert-error mt-4">
      <span>{{ error }}</span>
    </div>

    <div v-else-if="posts.length === 0" class="text-center py-16 opacity-50">
      <p class="text-lg">No posts yet</p>
      <p class="text-sm mt-1">Be the first to share something!</p>
    </div>

    <div v-else class="space-y-4">
      <PostCard v-for="post in posts" :key="post.id" :post="post" />

      <div v-if="hasMore" class="flex justify-center py-4">
        <button class="btn btn-outline btn-sm" :disabled="loadingMore" @click="loadMore">
          <span v-if="loadingMore" class="loading loading-spinner loading-sm"></span>
          <span v-else>Load more</span>
        </button>
      </div>
    </div>
  </main>
</template>
