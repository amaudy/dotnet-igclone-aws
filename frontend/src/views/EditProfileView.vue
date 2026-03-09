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
  <main class="max-w-lg mx-auto pt-16 pb-20 px-4">
    <h2 class="text-xl font-bold my-4">Edit Profile</h2>

    <LoadingSpinner v-if="loading" />

    <div v-else>
      <div v-if="error" role="alert" class="alert alert-error mb-4">
        <span>{{ error }}</span>
      </div>

      <form @submit.prevent="submit" class="space-y-4">
        <label class="form-control w-full">
          <div class="label"><span class="label-text">Display Name</span></div>
          <input
            v-model="displayName"
            type="text"
            class="input input-bordered w-full"
            maxlength="100"
          />
        </label>

        <label class="form-control w-full">
          <div class="label"><span class="label-text">Bio</span></div>
          <textarea
            v-model="bio"
            class="textarea textarea-bordered w-full"
            maxlength="500"
            rows="3"
          ></textarea>
        </label>

        <button
          type="submit"
          class="btn btn-primary w-full"
          :disabled="saving"
        >
          <span v-if="saving" class="loading loading-spinner loading-sm"></span>
          <span v-else>Save</span>
        </button>
      </form>
    </div>
  </main>
</template>
