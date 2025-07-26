import axios from 'axios'

// 创建 axios 实例，统一使用环境变量中的 API 地址
const http = axios.create({
  baseURL: import.meta.env.VITE_API_BASE_URL,   // ✅ 来自 .env 文件
  timeout: 10000,
  headers: {
    'Content-Type': 'application/json'
  }
})

// 自动带上 JWT Token
http.interceptors.request.use((config) => {
  const token = localStorage.getItem('token')
  if (token) {
    config.headers.Authorization = `Bearer ${token}`
  }
  return config
})

export default http
