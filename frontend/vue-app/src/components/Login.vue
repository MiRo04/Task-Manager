<script setup>
import { ref, defineEmits } from 'vue'
import { useRouter } from 'vue-router'

const token = ref(null)
const email = ref('')
const password = ref('')
const loading = ref(false)
const message = ref('')
const success = ref(false)

const router = useRouter()

const emit = defineEmits(['loginSuccess'])

const handleLogin = async () => {
  loading.value = true
  message.value = ''
  try {
    const response = await fetch('https://localhost:7181/api/users/login', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({
        email: email.value,
        password: password.value,
      }),
    })

    const data = await response.json()

    if (!response.ok) {
      message.value = 'Nieprawidłowy email lub hasło'
    } else {
      token.value = data.token
      localStorage.setItem('id', data.id)
      localStorage.setItem('token', data.token)
      localStorage.setItem('userName', data.userame)
      localStorage.setItem('email', data.email)

      success.value = true
      message.value = 'Zalogowano pomyślnie!'
      emit('loginSuccess')
      setTimeout(() => {
        router.push('/tasks')
      }, 1000)
    }
  } catch (error) {
    message.value = error.message
    success.value = false
  } finally {
    loading.value = false
  }
}
</script>
<template>
  <div class="container mt-5">
    <div class="row justify-content-center">
      <div class="col-md-6">
        <h2 class="mb-4 text-center">Logowanie</h2>
        <form @submit.prevent="handleLogin">
          <div class="mb-3">
            <label for="email" class="form-label">E-mail</label>
            <input type="email" v-model="email" class="form-control" id="email" required />
          </div>
          <div class="mb-3">
            <label for="password" class="form-label">Hasło</label>
            <input type="password" v-model="password" class="form-control" id="password" required />
          </div>
          <button type="submit" class="btn btn-primary w-100" :disabled="loading">
            {{ loading ? 'Logowanie...' : 'Zaloguj się' }}
          </button>

          <div
            v-if="message"
            class="alert mt-3"
            :class="{ 'alert-success': success, 'alert-danger': !success }"
          >
            {{ message }}
          </div>
        </form>
      </div>
    </div>
  </div>
</template>
