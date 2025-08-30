import http from '@/services/http.js' 

export function getBooks(keyword) {
return http.get('/book/search', {
    params: { keyword }
})
}

//创建书单
export function createBooklist(data) {
  return http.post('/book/booklists', data)
}

//删除书单
export function deleteBooklist(booklistId) {
  return http.delete(`/book/booklists/${booklistId}`)
}

//获取书单详情
export function getBooklistDetails(booklistId) {
  return http.get(`/book/booklists/${booklistId}`)
}

//推荐书单
export function recommendBooklists(booklistId, limit = 10) {
  return http.get(`/book/booklists/${booklistId}/recommend`, {
    params: { limit }
  })
}

//添加图书到书单
export function addBookToBooklist(booklistId, data) {
  return http.post(`/book/booklists/${booklistId}/books`, data)
}

//从书单移除图书
export function removeBookFromBooklist(booklistId, isbn) {
  return http.delete(`/book/booklists/${booklistId}/books/${isbn}`)
}

//收藏书单
export function collectBooklist(booklistId, data) {
  return http.post(`/book/booklists/${booklistId}/collect`, data)
}

//取消收藏书单
export function cancelCollectBooklist(booklistId) {
  return http.delete(`/book/booklists/${booklistId}/collect`)
}

//更新收藏备注
export function updateCollectNotes(booklistId, data) {
  return http.put(`/book/booklists/${booklistId}/collect/notes`, data)
}

//查询某个用户的书单（创建 + 收藏）
export function searchBooklistsByReader(readerId) {
  return http.get(`/book/booklists/reader/${readerId}`)
}

//修改书单名称
export function updateBooklistName(booklistId, data) {
  return http.put(`/book/booklists/${booklistId}/name`, data)
}

//修改书单简介
export function updateBooklistIntro(booklistId, data) {
  return http.put(`/book/booklists/${booklistId}/intro`, data)
}