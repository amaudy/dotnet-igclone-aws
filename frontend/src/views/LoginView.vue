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
    <div class="card bg-base-200 w-full max-w-sm shadow-xl">
      <div class="card-body">
        <h2 class="card-title justify-center text-2xl mb-4">InstaClone</h2>

        <div v-if="error" role="alert" class="alert alert-error mb-4">
          <span>{{ error }}</span>
        </div>

        <form @submit.prevent="submit" class="space-y-4">
          <label class="input input-bordered flex items-center gap-2 w-full">
            <input
              v-model="email"
              type="email"
              placeholder="Email"
              required
              class="grow"
            />
          </label>

          <label class="input input-bordered flex items-center gap-2 w-full">
            <input
              v-model="password"
              type="password"
              placeholder="Password"
              required
              minlength="6"
              class="grow"
            />
          </label>

          <button
            type="submit"
            class="btn btn-primary w-full"
            :disabled="loading"
          >
            <span v-if="loading" class="loading loading-spinner loading-sm"></span>
            <span v-else>Log In</span>
          </button>
        </form>

        <p class="text-center mt-4 text-sm">
          Don't have an account?
          <RouterLink to="/register" class="link link-primary">Sign up</RouterLink>
        </p>
      </div>
    </div>
  </div>
</template>
