import axios from 'axios'
import {useRouter} from 'vue-router'

const router = useRouter()

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
  if(config.withToken) {
    const token = localStorage.getItem('token')
    if (token) {
      config.headers.Authorization = `Bearer ${token}`
    }
  }
  return config
})

// 响应拦截器
http.interceptors.response.use(
  response => response,
  error => {
    // 如果后端返回的是 401（未认证）
    if (error.response?.status === 401) {
      // 清除本地 Token
      localStorage.removeItem('token')

      localStorage.removeItem('user')
      //给出提示
      alert('登录已过期，请重新登录。')

      // 跳转到登录页，并带上当前页地址用于回跳
      const currentPath = router.currentRoute.value.fullPath
      router.push({ path: '/login', query: { redirect: currentPath } })
    }
    else if(error.response?.status === 400)
    {
      //400BadRequest错误给出提示
      alert(error.response.message)
    }
    // 其他错误照常抛出
    return Promise.reject(error)
  }
)




export default http
