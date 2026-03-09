<script setup lang="ts">
import { useRoute, useRouter } from 'vue-router'
import { useAuthStore } from '../stores/auth'

const route = useRoute()
const router = useRouter()
const auth = useAuthStore()
</script>

<template>
  <div class="fixed bottom-0 left-1/2 -translate-x-1/2 w-full max-w-lg bg-gray-900 z-50 flex border-t border-gray-800">
    <button
      class="flex-1 flex justify-center items-center py-3 transition-colors"
      :class="route.path === '/' ? 'text-blue-500' : 'text-gray-500 hover:text-gray-300'"
      @click="router.push('/')"
    >
      <svg xmlns="http://www.w3.org/2000/svg" class="h-6 w-6" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
        <path d="M3 9l9-7 9 7v11a2 2 0 01-2 2H5a2 2 0 01-2-2z" />
        <polyline points="9 22 9 12 15 12 15 22" />
      </svg>
    </button>

    <button
      class="flex-1 flex justify-center items-center py-3 transition-colors"
      :class="route.path === '/create' ? 'text-blue-500' : 'text-gray-500 hover:text-gray-300'"
      @click="router.push('/create')"
    >
      <svg xmlns="http://www.w3.org/2000/svg" class="h-6 w-6" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
        <circle cx="12" cy="12" r="10" />
        <line x1="12" y1="8" x2="12" y2="16" />
        <line x1="8" y1="12" x2="16" y2="12" />
      </svg>
    </button>

    <button
      class="flex-1 flex justify-center items-center py-3 transition-colors"
      :class="route.path.startsWith('/profile') ? 'text-blue-500' : 'text-gray-500 hover:text-gray-300'"
      @click="router.push(`/profile/${auth.username}`)"
    >
      <svg xmlns="http://www.w3.org/2000/svg" class="h-6 w-6" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
        <path d="M20 21v-2a4 4 0 00-4-4H8a4 4 0 00-4 4v2" />
        <circle cx="12" cy="7" r="4" />
      </svg>
    </button>
  </div>
</template>
