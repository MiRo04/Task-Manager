<script setup>
import { ref } from 'vue'
import { useRouter } from 'vue-router'

const username = ref('')
const email = ref('')
const password = ref('')
const loading = ref(false)
const message = ref('')
const success = ref(false)

const router = useRouter()

const handleRegister = async () => {
  loading.value = true
  message.value = ''
  try {
    const response = await fetch('https://localhost:7181/api/users/register', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({
        username: username.value,
        email: email.value,
        password: password.value,
      }),
    })

    const data = await response.json()

    if (!response.ok) {
      throw new Error(data.message || 'Błąd rejestracji')
    }

    localStorage.setItem('token', data.token)

    success.value = true
    message.value = 'Rejestracja zakończona sukcesem! Przekierowanie...'
    setTimeout(() => {
      router.push('/login')
    }, 1500)
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
        <h2 class="mb-4 text-center">Rejestracja</h2>
        <form @submit.prevent="handleRegister">
          <div class="mb-3">
            <label for="name" class="form-label">Nazwa użytkownika</label>
            <input type="text" v-model="username" class="form-control" id="username" required />
          </div>
          <div class="mb-3">
            <label for="email" class="form-label">Adres e-mail</label>
            <input type="email" v-model="email" class="form-control" id="email" required />
          </div>
          <div class="mb-3">
            <label for="password" class="form-label">Hasło</label>
            <input type="password" v-model="password" class="form-control" id="password" required />
          </div>
          <button type="submit" class="btn btn-success w-100" :disabled="loading">
            {{ loading ? 'Rejestrowanie...' : 'Zarejestruj się' }}
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
