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
  <main class="max-w-lg mx-auto pt-16 pb-20 px-4">
    <h2 class="text-xl font-bold my-4">New Post</h2>

    <div v-if="error" role="alert" class="alert alert-error mb-4">
      <span>{{ error }}</span>
    </div>

    <form @submit.prevent="submit" class="space-y-4">
      <input
        type="file"
        accept="image/*"
        class="file-input file-input-bordered w-full"
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
        class="textarea textarea-bordered w-full"
        placeholder="Write a caption..."
        maxlength="2200"
        rows="3"
      ></textarea>

      <button
        type="submit"
        class="btn btn-primary w-full"
        :disabled="loading || !file"
      >
        <span v-if="loading" class="loading loading-spinner loading-sm"></span>
        <span v-else>Share</span>
      </button>
    </form>
  </main>
</template>
