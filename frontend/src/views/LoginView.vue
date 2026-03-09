<script setup lang="ts">
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import { useAuthStore } from '../stores/auth'
import type { ErrorResponse } from '../types'
import { AxiosError } from 'axios'

const auth = useAuthStore()
const router = useRouter()

const email = ref('')
const password = ref('')
const error = ref('')
const loading = ref(false)

async function submit() {
  error.value = ''
  loading.value = true
  try {
    await auth.login({ email: email.value, password: password.value })
    router.push('/')
  } catch (e) {
    if (e instanceof AxiosError && e.response?.data) {
      const data = e.response.data as ErrorResponse
      error.value = data.message || 'Login failed'
    } else {
      error.value = 'Network error'
    }
  } finally {
    loading.value = false
  }
}
</script>

<template>
  <div class="flex items-center justify-center min-h-screen px-4">
    <div class="w-full max-w-sm bg-gray-900 rounded-xl shadow-xl p-6">
      <h2 class="text-2xl font-bold text-center mb-6">InstaClone</h2>

      <div v-if="error" class="p-3 rounded-lg bg-red-900/50 text-red-300 text-sm mb-4">
        {{ error }}
      </div>

      <form @submit.prevent="submit" class="space-y-4">
        <input
          v-model="email"
          type="email"
          placeholder="Email"
          required
          class="w-full px-3 py-2 bg-gray-800 border border-gray-700 rounded-lg focus:outline-none focus:border-blue-500"
        />

        <input
          v-model="password"
          type="password"
          placeholder="Password"
          required
          minlength="6"
          class="w-full px-3 py-2 bg-gray-800 border border-gray-700 rounded-lg focus:outline-none focus:border-blue-500"
        />

        <button
          type="submit"
          class="w-full py-2 bg-blue-600 text-white rounded-lg font-medium hover:bg-blue-700 disabled:opacity-50"
          :disabled="loading"
        >
          <span v-if="loading" class="spinner h-4 w-4"></span>
          <span v-else>Log In</span>
        </button>
      </form>

      <p class="text-center mt-4 text-sm text-gray-400">
        Don't have an account?
        <RouterLink to="/register" class="text-blue-500 hover:underline">Sign up</RouterLink>
      </p>
    </div>
  </div>
</template>
