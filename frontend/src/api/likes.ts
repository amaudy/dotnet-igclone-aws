import client from './client'

export function likePost(postId: number) {
  return client.post(`/posts/${postId}/like`)
}

export function unlikePost(postId: number) {
  return client.delete(`/posts/${postId}/like`)
}
