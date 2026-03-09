<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import { getProfile, updateProfile } from '../api/profiles'
import { useAuthStore } from '../stores/auth'
import type { ErrorResponse } from '../types'
import { AxiosError } from 'axios'
import AppHeader from '../components/AppHeader.vue'
import LoadingSpinner from '../components/LoadingSpinner.vue'

const auth = useAuthStore()
const router = useRouter()

const displayName = ref('')
const bio = ref('')
const loading = ref(true)
const saving = ref(false)
const error = ref('')

onMounted(async () => {
  try {
    const res = await getProfile(auth.username)
    displayName.value = res.data.displayName ?? ''
    bio.value = res.data.bio ?? ''
  } catch {
    error.value = 'Failed to load profile'
  } finally {
    loading.value = false
  }
})

async function submit() {
  error.value = ''
  saving.value = true
  try {
    await updateProfile({
      displayName: displayName.value,
      bio: bio.value,
    })
    router.push(`/profile/${auth.username}`)
  } catch (e) {
    if (e instanceof AxiosError && e.response?.data) {
      const data = e.response.data as ErrorResponse
      error.value = data.message || 'Update failed'
    } else {
      error.value = 'Network error'
    }
  } finally {
    saving.value = false
  }
}
</script>

<template>
  <AppHeader />
  <main class="pb-20 px-4">
    <h2 class="text-xl font-bold my-4">Edit Profile</h2>

    <LoadingSpinner v-if="loading" />

    <div v-else>
      <div v-if="error" class="p-3 rounded-lg bg-red-900/50 text-red-300 text-sm mb-4">
        {{ error }}
      </div>

      <form @submit.prevent="submit" class="space-y-4">
        <div>
          <label class="block text-sm text-gray-400 mb-1">Display Name</label>
          <input
            v-model="displayName"
            type="text"
            class="w-full px-3 py-2 bg-gray-800 border border-gray-700 rounded-lg focus:outline-none focus:border-blue-500"
            maxlength="100"
          />
        </div>

        <div>
          <label class="block text-sm text-gray-400 mb-1">Bio</label>
          <textarea
            v-model="bio"
            class="w-full px-3 py-2 bg-gray-800 border border-gray-700 rounded-lg focus:outline-none focus:border-blue-500 resize-none"
            maxlength="500"
            rows="3"
          ></textarea>
        </div>

        <button
          type="submit"
          class="w-full py-2 bg-blue-600 text-white rounded-lg font-medium hover:bg-blue-700 disabled:opacity-50"
          :disabled="saving"
        >
          <span v-if="saving" class="spinner h-4 w-4"></span>
          <span v-else>Save</span>
        </button>
      </form>
    </div>
  </main>
</template>
