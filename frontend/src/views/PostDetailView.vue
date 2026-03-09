<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useRoute } from 'vue-router'
import { getPost } from '../api/posts'
import { likePost, unlikePost } from '../api/likes'
import { getComments, createComment, deleteComment } from '../api/comments'
import { useAuthStore } from '../stores/auth'
import type { PostResponse, CommentResponse } from '../types'
import AppHeader from '../components/AppHeader.vue'
import CommentItem from '../components/CommentItem.vue'
import LoadingSpinner from '../components/LoadingSpinner.vue'

const route = useRoute()
const auth = useAuthStore()
const postId = Number(route.params.id)

const post = ref<PostResponse | null>(null)
const comments = ref<CommentResponse[]>([])
const loading = ref(true)
const liked = ref(false)
const localLikeCount = ref(0)
const commentText = ref('')
const submitting = ref(false)

onMounted(async () => {
  try {
    const [postRes, commentsRes] = await Promise.all([
      getPost(postId),
      getComments(postId),
    ])
    post.value = postRes.data
    localLikeCount.value = postRes.data.likeCount
    comments.value = commentsRes.data
  } catch {
    // handled by empty state
  } finally {
    loading.value = false
  }
})

async function toggleLike() {
  if (liked.value) {
    liked.value = false
    localLikeCount.value--
    try { await unlikePost(postId) } catch { liked.value = true; localLikeCount.value++ }
  } else {
    liked.value = true
    localLikeCount.value++
    try { await likePost(postId) } catch { liked.value = false; localLikeCount.value-- }
  }
}

async function addComment() {
  if (!commentText.value.trim()) return
  submitting.value = true
  try {
    const res = await createComment(postId, { text: commentText.value.trim() })
    comments.value.unshift(res.data)
    commentText.value = ''
    if (post.value) post.value.commentCount++
  } catch {
    // silent
  } finally {
    submitting.value = false
  }
}

async function onDeleteComment(id: number) {
  try {
    await deleteComment(id)
    comments.value = comments.value.filter((c) => c.id !== id)
    if (post.value) post.value.commentCount--
  } catch {
    // silent
  }
}

function timeAgo(dateStr: string): string {
  const seconds = Math.floor((Date.now() - new Date(dateStr).getTime()) / 1000)
  if (seconds < 60) return 'just now'
  const minutes = Math.floor(seconds / 60)
  if (minutes < 60) return `${minutes}m ago`
  const hours = Math.floor(minutes / 60)
  if (hours < 24) return `${hours}h ago`
  return `${Math.floor(hours / 24)}d ago`
}
</script>

<template>
  <AppHeader />
  <main class="pb-20 px-4">
    <LoadingSpinner v-if="loading" />

    <div v-else-if="!post" class="text-center py-16 opacity-50">
      Post not found
    </div>

    <div v-else>
      <div class="flex items-center gap-3 py-3">
        <div class="avatar placeholder">
          <div class="bg-neutral text-neutral-content w-8 h-8 rounded-full flex items-center justify-center">
            <span class="text-sm">{{ post.username.charAt(0).toUpperCase() }}</span>
          </div>
        </div>
        <RouterLink :to="`/profile/${post.username}`" class="font-semibold text-sm hover:underline">
          {{ post.username }}
        </RouterLink>
      </div>

      <img :src="post.imageUrl" :alt="post.caption ?? 'Post'" class="w-full rounded-lg" />

      <div class="flex items-center gap-3 py-3">
        <button class="btn btn-ghost btn-sm btn-circle" @click="toggleLike">
          <svg xmlns="http://www.w3.org/2000/svg" class="h-6 w-6" :class="liked ? 'fill-error text-error' : ''" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
            <path d="M20.84 4.61a5.5 5.5 0 00-7.78 0L12 5.67l-1.06-1.06a5.5 5.5 0 00-7.78 7.78l1.06 1.06L12 21.23l7.78-7.78 1.06-1.06a5.5 5.5 0 000-7.78z" />
          </svg>
        </button>
        <span class="text-sm font-semibold">{{ localLikeCount }} {{ localLikeCount === 1 ? 'like' : 'likes' }}</span>
      </div>

      <p v-if="post.caption" class="text-sm mb-1">
        <span class="font-semibold">{{ post.username }}</span>
        {{ post.caption }}
      </p>
      <p class="text-xs opacity-50 mb-4">{{ timeAgo(post.createdAt) }}</p>

      <div class="divider my-2"></div>

      <div v-if="comments.length === 0" class="text-center py-4 opacity-50 text-sm">
        No comments yet
      </div>
      <div v-else>
        <CommentItem
          v-for="comment in comments"
          :key="comment.id"
          :comment="comment"
          :current-user="auth.username"
          @delete="onDeleteComment"
        />
      </div>

      <form @submit.prevent="addComment" class="flex gap-2 mt-4">
        <input
          v-model="commentText"
          type="text"
          placeholder="Add a comment..."
          class="input input-bordered input-sm flex-1"
          maxlength="500"
        />
        <button
          type="submit"
          class="btn btn-primary btn-sm"
          :disabled="!commentText.trim() || submitting"
        >
          <span v-if="submitting" class="loading loading-spinner loading-xs"></span>
          <span v-else>Post</span>
        </button>
      </form>
    </div>
  </main>
</template>
