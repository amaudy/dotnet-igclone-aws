<script setup lang="ts">
import { useAuthStore } from './stores/auth'
import BottomNavbar from './components/BottomNavbar.vue'
import AppToast from './components/AppToast.vue'

const auth = useAuthStore()
</script>

<template>
  <div class="min-h-screen bg-base-100">
    <div class="max-w-lg mx-auto relative">
      <RouterView />
      <BottomNavbar v-if="auth.isAuthenticated" />
    </div>
  </div>
  <AppToast />
</template>
