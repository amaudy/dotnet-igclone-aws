<script setup lang="ts">
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import { createPost } from '../api/posts'
import type { ErrorResponse } from '../types'
import { AxiosError } from 'axios'
import AppHeader from '../components/AppHeader.vue'

const router = useRouter()

const file = ref<File | null>(null)
const preview = ref('')
const caption = ref('')
const error = ref('')
const loading = ref(false)

function onFileChange(e: Event) {
  const target = e.target as HTMLInputElement
  const f = target.files?.[0]
  if (!f) return
  file.value = f
  preview.value = URL.createObjectURL(f)
}

async function submit() {
  if (!file.value) return
  error.value = ''
  loading.value = true
  try {
    const formData = new FormData()
    formData.append('Image', file.value)
    if (caption.value) formData.append('Caption', caption.value)
    await createPost(formData)
    router.push('/')
  } catch (e) {
    if (e instanceof AxiosError && e.response?.data) {
      const data = e.response.data as ErrorResponse
      error.value = data.message || 'Upload failed'
    } else {
      error.value = 'Network error'
    }
  } finally {
    loading.value = false
  }
}
</script>

<template>
  <AppHeader />
  <main class="pb-20 px-4">
    <h2 class="text-xl font-bold my-4">New Post</h2>

    <div v-if="error" class="p-3 rounded-lg bg-red-900/50 text-red-300 text-sm mb-4">
      {{ error }}
    </div>

    <form @submit.prevent="submit" class="space-y-4">
      <input
        type="file"
        accept="image/*"
        class="block w-full text-sm text-gray-400 file:mr-4 file:py-2 file:px-4 file:rounded-lg file:border-0 file:bg-gray-800 file:text-gray-300 hover:file:bg-gray-700"
        @change="onFileChange"
        required
      />

      <img
        v-if="preview"
        :src="preview"
        alt="Preview"
        class="rounded-lg w-full object-cover max-h-96"
      />

      <textarea
        v-model="caption"
        class="w-full px-3 py-2 bg-gray-800 border border-gray-700 rounded-lg focus:outline-none focus:border-blue-500 resize-none"
        placeholder="Write a caption..."
        maxlength="2200"
        rows="3"
      ></textarea>

      <button
        type="submit"
        class="w-full py-2 bg-blue-600 text-white rounded-lg font-medium hover:bg-blue-700 disabled:opacity-50"
        :disabled="loading || !file"
      >
        <span v-if="loading" class="spinner h-4 w-4"></span>
        <span v-else>Share</span>
      </button>
    </form>
  </main>
</template>
