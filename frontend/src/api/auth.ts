import client from './client'
import type { LoginRequest, RegisterRequest, TokenResponse } from '../types'

export function login(data: LoginRequest) {
  return client.post<TokenResponse>('/auth/login', data)
}

export function register(data: RegisterRequest) {
  return client.post<TokenResponse>('/auth/register', data)
}
