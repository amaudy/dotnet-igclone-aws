import { defineStore } from 'pinia'
import { ref, computed } from 'vue'
import * as authApi from '../api/auth'
import type { LoginRequest, RegisterRequest } from '../types'

function decodePayload(token: string): { unique_name: string; [key: string]: string } {
  const payload = token.split('.')[1]
  return JSON.parse(atob(payload))
}

export const useAuthStore = defineStore('auth', () => {
  const token = ref(localStorage.getItem('token'))
  const username = ref('')

  if (token.value) {
    try {
      username.value = decodePayload(token.value).unique_name ?? ''
    } catch {
      token.value = null
      localStorage.removeItem('token')
    }
  }

  const isAuthenticated = computed(() => !!token.value)

  function setToken(t: string) {
    token.value = t
    localStorage.setItem('token', t)
    username.value = decodePayload(t).unique_name ?? ''
  }

  async function login(data: LoginRequest) {
    const res = await authApi.login(data)
    setToken(res.data.token)
  }

  async function register(data: RegisterRequest) {
    const res = await authApi.register(data)
    setToken(res.data.token)
  }

  function logout() {
    token.value = null
    username.value = ''
    localStorage.removeItem('token')
  }

  return { token, username, isAuthenticated, login, register, logout }
})
