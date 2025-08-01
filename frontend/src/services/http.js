import axios from 'axios'

// 创建 axios 实例，统一使用环境变量中的 API 地址
const http = axios.create({
  baseURL: 'http://localhost:5000',   // ✅ 直接设置后端API地址
  timeout: 10000,
  headers: {
    'Content-Type': 'application/json'
  }
})

export default http
