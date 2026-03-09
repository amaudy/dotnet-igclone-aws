import axios from 'axios'
import { useToastStore } from '../stores/toast'

const client = axios.create({
  baseURL: '/api',
})

client.interceptors.request.use((config) => {
  const token = localStorage.getItem('token')
  if (token) {
    config.headers.Authorization = `Bearer ${token}`
  }
  return config
})

client.interceptors.response.use(
  (response) => response,
  (error) => {
    if (error.response?.status === 401) {
      localStorage.removeItem('token')
      window.location.href = '/login'
    } else if (!error.response) {
      try {
        const toast = useToastStore()
        toast.show('Network error. Please check your connection.')
      } catch {
        // store not yet initialized
      }
    }
    return Promise.reject(error)
  },
)

export default client
