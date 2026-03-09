import client from './client'
import type { PaginatedResponse, PostResponse } from '../types'

export function getPosts(page = 1, pageSize = 20, username?: string) {
  return client.get<PaginatedResponse<PostResponse>>('/posts', {
    params: { page, pageSize, username },
  })
}

export function getPost(id: number) {
  return client.get<PostResponse>(`/posts/${id}`)
}

export function createPost(data: FormData) {
  return client.post<PostResponse>('/posts', data, {
    headers: { 'Content-Type': 'multipart/form-data' },
  })
}

export function deletePost(id: number) {
  return client.delete(`/posts/${id}`)
}
