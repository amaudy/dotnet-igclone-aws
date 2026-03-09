import client from './client'
import type { ProfileResponse, UpdateProfileRequest } from '../types'

export function getProfile(username: string) {
  return client.get<ProfileResponse>(`/profiles/${username}`)
}

export function updateProfile(data: UpdateProfileRequest) {
  return client.put<ProfileResponse>('/profiles', data)
}
