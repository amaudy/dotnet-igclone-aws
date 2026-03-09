import client from './client'
import type { CommentResponse, CreateCommentRequest } from '../types'

export function getComments(postId: number) {
  return client.get<CommentResponse[]>(`/posts/${postId}/comments`)
}

export function createComment(postId: number, data: CreateCommentRequest) {
  return client.post<CommentResponse>(`/posts/${postId}/comments`, data)
}

export function deleteComment(id: number) {
  return client.delete(`/comments/${id}`)
}
