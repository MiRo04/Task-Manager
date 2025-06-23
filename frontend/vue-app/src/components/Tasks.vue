<script setup>
import { ref, onMounted } from 'vue'

const tasks = ref([])
const message = ref('')
const messageType = ref('')
const title = ref('')
const description = ref('')
const date = ref('')
const time = ref('')

onMounted(async () => {
  loadTasks()
})

async function loadTasks() {
  const token = localStorage.getItem('token')
  const res = await fetch(`https://localhost:7181/api/tasks?userId=${localStorage.getItem('id')}`, {
    headers: {
      Authorization: `Bearer ${token}`,
    },
  })
  if (res.ok) {
    tasks.value = await res.json()
  } else {
    message.value = 'Nie posiadasz żadnych zadań'
    messageType.value = 'info'
  }
}
async function addTask() {
  const token = localStorage.getItem('token')
  const userId = localStorage.getItem('id')

  if (!title.value || !description.value || !date.value || !time.value) {
    message.value = 'Wypełnij wszystkie pola'
    messageType.value = 'warning'
    return
  }

  const task = {
    title: title.value,
    description: description.value,
    dueDate: new Date(`${date.value}T${time.value}`).toISOString(),
  }

  const res = await fetch(`https://localhost:7181/api/tasks/add/${userId}`, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
      Authorization: `Bearer ${token}`,
    },
    body: JSON.stringify(task),
  })

  if (res.ok) {
    title.value = ''
    description.value = ''
    date.value = ''
    time.value = ''
    message.value = 'Zadanie zostało dodane'
    messageType.value = 'success'
    await loadTasks()
  } else if (res.status == 400) {
    message.value = 'Wprowadzane dane są nieprawidłowe'
    messageType.value = 'warning'
  } else {
    message.value = 'Wystąpił błąd! Nie udało się dodać zadania'
    messageType.value = 'danger'
  }
}
// usuwania zadania
const deleteTask = async (taskId) => {
  const confirmed = confirm('Czy na pewno chcesz usunąć to zadanie?')
  if (!confirmed) return

  const token = localStorage.getItem('token')

  try {
    const res = await fetch(`https://localhost:7181/api/tasks/${taskId}`, {
      method: 'DELETE',
      headers: {
        Authorization: `Bearer ${token}`,
      },
    })

    if (res.ok) {
      message.value = 'Zadanie zostało usunięte.'
      messageType.value = 'success'
      tasks.value = tasks.value.filter((task) => task.id !== taskId)
      await loadTasks()
    } else {
      message.value = 'Nie udało się usunąć zadania.'
      messageType.value = 'danger'
    }
  } catch (err) {
    console.error(err)
    message.value = 'Wystąpił błąd przy próbie usunięcia zadania.'
    messageType.value = 'danger'
  }
}
</script>

<template>
  <div class="container mt-3">
    <div class="d-flex flex-column">
      <h2 class="mb-3">Dodaj zadanie</h2>
      <form @submit.prevent="addTask" class="d-flex w-100">
        <div class="d-flex flex-column w-50">
          <div class="d-flex flex-row w-100 mb-3">
            <label style="width: 100px">Tytuł: </label>
            <input type="text" name="title" v-model="title" id="title" />
          </div>
          <div class="d-flex flex-row w-100 mb-3">
            <label for="description" style="width: 100px">Opis: </label>
            <textarea
              type="text"
              name="description"
              v-model="description"
              id="description"
            ></textarea>
          </div>
          <div class="d-flex flex-row w-100 mb-3">
            <label for="dueDate" style="width: 100px">Do kiedy: </label>
            <input type="date" name="date" id="date" v-model="date" />
            <input type="time" name="time" id="time" v-model="time" />
          </div>
        </div>
        <div class="d-flex w-50 flex-column">
          <button type="submit" class="btn btn-success btn-md mt-3">Dodaj zadanie</button>
        </div>
      </form>
    </div>
    <div class="d-flex flex-column">
      <div v-if="message" :class="`alert alert-${messageType || 'info'}`">
        {{ message }}
      </div>
      <hr />
      <!-- Lista zadań -->
      <div v-if="tasks.length > 0" class="row row-cols-1 row-cols-md-2 g-4">
        <div v-for="task in tasks" :key="task.id" class="col">
          <div class="card h-100 border-primary shadow-sm">
            <div class="card-body">
              <h5 class="card-title">
                {{ task.title }}
                <span class="badge bg-primary float-end">ID: {{ task.id }}</span>
              </h5>
              <p class="card-text">{{ task.description }}</p>
            </div>
            <div class="card-footer d-flex justify-content-between align-items-center">
              <small class="text-muted">
                Do wykonania: {{ new Date(task.dueDate).toLocaleString() }}
              </small>
              <button @click="deleteTask(task.id)" class="btn btn-sm btn-outline-danger ms-auto">
                Usuń
              </button>
            </div>
          </div>
        </div>
      </div>

      <!-- Gdy nie ma zadań -->
      <div v-else class="alert alert-warning">Brak zadań do wyświetlenia.</div>
    </div>
  </div>
</template>
