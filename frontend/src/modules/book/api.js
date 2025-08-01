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