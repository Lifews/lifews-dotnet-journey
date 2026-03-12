<script setup>
import { ref } from 'vue'
import axios from 'axios'

const username = ref('')
const password = ref('')
const loginResult = ref({})

async function login() {
    try {
        const response = await axios.post('https://localhost:7133/api/Login/Login', {
            username: username.value,
            password: password.value,
        })

        loginResult.value = response.data
    } catch (error) {
        console.error('请求失败', error)
        loginResult.value = { ok: false }
    }
}
</script>

<template>
    <div>
        <h2>登录</h2>
        <form @submit.prevent="login">
            <div>
                <label>用户名：</label>
                <input v-model="username" type="text" required />
            </div>
            <div>
                <label>密码：</label>
                <input v-model="password" type="password" required />
            </div>
            <button type="submit">登录</button>
        </form>

        <div v-if="loginResult.ok">
            <h3>进程列表：</h3>
            <ul>
                <li v-for="proc in loginResult.processInfos" :key="proc.id">
                    {{ proc.name }} (PID: {{ proc.id }}) - 内存: {{ proc.workingSet }} bytes
                </li>
            </ul>
        </div>
        <div v-else-if="loginResult.ok === false">
            <p style="color: red;">登录失败，请检查用户名或密码。</p>
        </div>
    </div>
</template>