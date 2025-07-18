import http from '@/services/http.js' 

export function getBooks(keyword) {
  return http.get('/book/search', {
    params: { keyword }
  })
}
