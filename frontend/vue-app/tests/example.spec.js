// @ts-check
import { test, expect } from '@playwright/test'

test('has title', async ({ page }) => {
  await page.goto('http://localhost:3000/')

  // Expect a title "to contain" a substring.
  await expect(page).toHaveTitle(/Task Manager/)
})

test('go to login', async ({ page }) => {
  await page.goto('http://localhost:3000/login')
  await expect(page).toHaveURL
})
test('login with wrong data', async ({ page }) => {
  await page.goto('http://localhost:3000/login')
  await page.getByLabel('E-mail').fill('nniepoprawny@adres.pl')
  await page.getByLabel('Hasło').fill('Nieporawnehaslo123@')
  await page.getByRole('button', { name: 'Zaloguj się' }).click()
  const [response] = await Promise.all([
    page.waitForResponse(
      (resp) => resp.url().includes('/api/login') && resp.request().method() === 'POST',
    ),
  ])

  expect(response.status().toBe(400))
})
test('go to register', async ({ page }) => {
  await page.goto('http://localhost:3000/register')
})
