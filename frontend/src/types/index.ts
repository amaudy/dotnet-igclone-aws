export interface TokenResponse {
  token: string
  expiration: string
}

export interface RegisterRequest {
  username: string
  email: string
  password: string
}

export interface LoginRequest {
  email: string
  password: string
}

export interface PostResponse {
  id: number
  imageUrl: string
  caption: string | null
  createdAt: string
  username: string
  likeCount: number
  commentCount: number
}

export interface PaginatedResponse<T> {
  items: T[]
  page: number
  pageSize: number
  totalCount: number
  totalPages: number
}

export interface ProfileResponse {
  username: string
  displayName: string | null
  bio: string | null
  avatarUrl: string | null
  postCount: number
}

export interface UpdateProfileRequest {
  displayName?: string
  bio?: string
  avatarUrl?: string
}

export interface CommentResponse {
  id: number
  text: string
  createdAt: string
  username: string
}

export interface CreateCommentRequest {
  text: string
}

export interface ErrorResponse {
  message: string
  errors: string[]
}
