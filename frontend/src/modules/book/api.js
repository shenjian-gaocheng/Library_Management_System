// frontend/src/modules/book/api.js

import http from '@/services/http.js'

export function getBooks(keyword) {
  return http.get('/book/search', {
    params: { keyword }
  })
}

export function getCommentsByISBN(ISBN) {
  return http.get('/comment/search', {
    params: { ISBN }
  })
}

export function addComment(commentData) {
  return http.post('/comment/add', commentData)
}

export function getCategoryTree() {
  return http.get('/category/tree')
}
export function addCategory(data) {
  return http.post('/category', data)
}
export function updateCategory(data) {
  return http.put('/category', data)
}
export function deleteCategory(id, operatorId) {
  return http.delete(`/category/${id}?operatorId=${operatorId}`)
}