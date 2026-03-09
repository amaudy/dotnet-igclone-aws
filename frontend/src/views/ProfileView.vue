<script setup lang="ts">
import { ref, onMounted, watch } from 'vue'
import { useRoute } from 'vue-router'
import { getProfile } from '../api/profiles'
import { getPosts } from '../api/posts'
import { useAuthStore } from '../stores/auth'
import type { ProfileResponse, PostResponse } from '../types'
import AppHeader from '../components/AppHeader.vue'
import LoadingSpinner from '../components/LoadingSpinner.vue'

const route = useRoute()
const auth = useAuthStore()

const profile = ref<ProfileResponse | null>(null)
const posts = ref<PostResponse[]>([])
const loading = ref(true)
const error = ref('')

async function load(username: string) {
  loading.value = true
  error.value = ''
  try {
    const [profileRes, postsRes] = await Promise.all([
      getProfile(username),
      getPosts(1, 50, username),
    ])
    profile.value = profileRes.data
    posts.value = postsRes.data.items
  } catch {
    error.value = 'Failed to load profile'
  } finally {
    loading.value = false
  }
}

onMounted(() => load(route.params.username as string))
watch(() => route.params.username, (u) => { if (u) load(u as string) })
</script>

<template>
  <AppHeader />
  <main class="pb-20 px-4">
    <LoadingSpinner v-if="loading" />

    <div v-else-if="error" class="p-3 rounded-lg bg-red-900/50 text-red-300 text-sm mt-4">
      {{ error }}
    </div>

    <div v-else-if="profile">
      <div class="flex items-center gap-4 py-6">
        <div class="w-16 h-16 rounded-full bg-gray-700 text-gray-200 flex items-center justify-center text-2xl font-medium">
          {{ profile.username.charAt(0).toUpperCase() }}
        </div>
        <div class="flex-1">
          <h2 class="text-xl font-bold">{{ profile.username }}</h2>
          <p v-if="profile.displayName" class="text-sm text-gray-400">{{ profile.displayName }}</p>
          <p v-if="profile.bio" class="text-sm mt-1">{{ profile.bio }}</p>
          <p class="text-sm text-gray-500 mt-1">{{ posts.length }} posts</p>
        </div>
      </div>

      <RouterLink
        v-if="profile.username === auth.username"
        to="/profile/edit"
        class="block text-center w-full py-1.5 text-sm border border-gray-700 rounded-lg hover:bg-gray-800 mb-4"
      >
        Edit Profile
      </RouterLink>

      <div v-if="posts.length === 0" class="text-center py-8 text-gray-500">
        No posts yet
      </div>
      <div v-else class="grid grid-cols-3 gap-1">
        <RouterLink
          v-for="post in posts"
          :key="post.id"
          :to="`/posts/${post.id}`"
          class="aspect-square"
        >
          <img :src="post.imageUrl" :alt="post.caption ?? ''" class="w-full h-full object-cover" />
        </RouterLink>
      </div>
    </div>
  </main>
</template>
