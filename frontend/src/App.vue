<script setup lang="ts">
import { useAuthStore } from './stores/auth'
import BottomNavbar from './components/BottomNavbar.vue'

const auth = useAuthStore()
</script>

<template>
  <div class="min-h-screen bg-base-100 max-w-lg mx-auto">
    <RouterView />
    <BottomNavbar v-if="auth.isAuthenticated" />
  </div>
</template>
