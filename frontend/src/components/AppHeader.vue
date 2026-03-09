<script setup lang="ts">
import { useRouter } from 'vue-router'
import { useAuthStore } from '../stores/auth'

const auth = useAuthStore()
const router = useRouter()

function logout() {
  auth.logout()
  router.push('/login')
}
</script>

<template>
  <div class="flex items-center justify-between px-4 py-3 bg-gray-900 sticky top-0 z-50">
    <RouterLink to="/" class="text-xl font-bold hover:opacity-80">InstaClone</RouterLink>
    <button class="text-sm text-gray-400 hover:text-white" @click="logout">Logout</button>
  </div>
</template>
