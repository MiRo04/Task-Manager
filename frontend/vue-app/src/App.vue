<script setup>
import Navbar from './components/Navbar.vue'
import { onMounted, ref } from 'vue'
import router from './router'

const navPositions = [
  { title: 'Zaloguj się', url: '/login' },
  { title: 'Zarejestruj się', url: '/register' },
]

let token = ref(localStorage.getItem('token'))

function onloginSuccess() {
  console.log('zalogowano')
  token.value = localStorage.getItem('token')
}
const handleLogout = () => {
  token.value = null
  localStorage.removeItem('id')
  localStorage.removeItem('token')
  localStorage.removeItem('userName')
  localStorage.removeItem('email')
  router.push('/login')
}
</script>

<template>
  <Navbar :links="navPositions" :token="token" @logout="handleLogout"></Navbar>
  <router-view @loginSuccess="onloginSuccess" />
</template>
