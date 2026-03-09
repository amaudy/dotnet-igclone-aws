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
  <div class="navbar bg-base-200 sticky top-0 z-50">
    <div class="flex-1">
      <RouterLink to="/" class="btn btn-ghost text-xl">InstaClone</RouterLink>
    </div>
    <div class="flex-none">
      <button class="btn btn-ghost btn-sm" @click="logout">Logout</button>
    </div>
  </div>
</template>
