<script setup>
import { ref, defineProps, defineEmits } from 'vue'

const emit = defineEmits(['logout'])

const props = defineProps({
  links: Array,
  token: String,
})

const handleLogoutClick = () => {
  emit('logout')
}
</script>
<template>
  <nav class="navbar navbar-dark navbar-expand-lg bg-body-dark">
    <div class="container-fluid">
      <router-link class="navbar-brand" to="/">Task Manager</router-link>
      <button
        class="navbar-toggler"
        type="button"
        data-bs-toggle="collapse"
        data-bs-target="#navbarNavAltMarkup"
        aria-controls="navbarNavAltMarkup"
        aria-expanded="false"
        aria-label="Toggle navigation"
      >
        <span class="navbar-toggler-icon"></span>
      </button>
      <div class="collapse navbar-collapse" id="navbarNavAltMarkup">
        <div class="navbar-nav">
          <template v-if="!token">
            <router-link
              v-for="(link, index) in links"
              :key="index"
              :to="link.url"
              class="nav-link"
              active-class="active"
            >
              {{ link.title }}
            </router-link>
          </template>
          <template v-else>
            <a class="nav-link" href="#" @click.prevent="handleLogoutClick">Wyloguj się</a>
            <RouterLink class="nav-link" to="/tasks">Zadania</RouterLink>
          </template>
        </div>
      </div>
    </div>
  </nav>
</template>
