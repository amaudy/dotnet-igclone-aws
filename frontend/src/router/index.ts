import { createRouter, createWebHistory } from 'vue-router'
import { useAuthStore } from '../stores/auth'

const router = createRouter({
  history: createWebHistory(),
  routes: [
    {
      path: '/login',
      component: () => import('../views/LoginView.vue'),
      meta: { guest: true },
    },
    {
      path: '/register',
      component: () => import('../views/RegisterView.vue'),
      meta: { guest: true },
    },
    {
      path: '/',
      component: () => import('../views/FeedView.vue'),
      meta: { auth: true },
    },
    {
      path: '/posts/:id',
      component: () => import('../views/PostDetailView.vue'),
      meta: { auth: true },
    },
    {
      path: '/create',
      component: () => import('../views/CreatePostView.vue'),
      meta: { auth: true },
    },
    {
      path: '/profile/edit',
      component: () => import('../views/EditProfileView.vue'),
      meta: { auth: true },
    },
    {
      path: '/profile/:username',
      component: () => import('../views/ProfileView.vue'),
      meta: { auth: true },
    },
    {
      path: '/profile',
      redirect: () => {
        const auth = useAuthStore()
        return `/profile/${auth.username}`
      },
      meta: { auth: true },
    },
  ],
})

router.beforeEach((to) => {
  const auth = useAuthStore()
  if (to.meta.auth && !auth.isAuthenticated) return '/login'
  if (to.meta.guest && auth.isAuthenticated) return '/'
})

export default router
