import http from '@/services/http.js'

// ---------- 图书相关 ----------
export function getBooks(keyword) {
  return http.get('/book/search', {
    params: { keyword }
  })
}

export function getBookById(bookId) {
  return http.get(`/Book/${bookId}`, { withToken: true })
}

export function getBookByBarcode(barcode) {
  return http.get(`/Book/by-barcode/${encodeURIComponent(barcode)}`, { withToken: true })
}

// ---------- 评论 ----------
export function getCommentsByISBN(ISBN) {
  return http.get('/comment/search', {
    params: { ISBN },
    withToken: true
  })
}

export function addComment(commentData) {
  return http.post('/comment/add', commentData, { withToken: true })
}

// ---------- 分类 ----------
export function getCategoryTree() {
  return http.get('/api/Category/tree', { withToken: true })
}

export function addCategory(data) {
  return http.post('/api/Category', data, { withToken: true })
}

export function updateCategory(data) {
  return http.put('/api/Category', data, { withToken: true })
}

export function deleteCategory(id, operatorId) {
  return http.delete(`/api/Category/${id}?operatorId=${operatorId}`, { withToken: true })
}

// ---------- 书架 ----------
export function getBooksBookShelf(keyword) {
  return http.get('/bookshelf/search_book_which_shelf', {
    params: { keyword },
    withToken: true
  })
}

export function getShelf(keyword) {
  return http.get('/bookshelf/search_bookshelf', {
    params: { keyword },
    withToken: true
  })
}

export function addShelf(buildingid, shelfcode, floor, zone) {
  return http.post('/bookshelf/add_bookshelf', {
    buildingid: Number(buildingid),
    shelfcode: shelfcode,
    floor: Number(floor),
    zone: zone
  }, { withToken: true })
}

export function deleteShelf(shelfId) {
  return http.delete(`/bookshelf/delete/${shelfId}`, { withToken: true })
}

export function checkShelfHasBooks(shelfId) {
  return http.get(`/bookshelf/has-books/${shelfId}`, { withToken: true })
}

export function checkShelfExists(buildingId, shelfCode, floor, zone) {
  return http.get('/bookshelf/check-shelf-exists', {
    params: { buildingId, shelfCode, floor, zone },
    withToken: true
  })
}

export function returnBook(bookId, shelfId) {
  return http.post('/bookshelf/return-book', {
    bookId: Number(bookId),
    shelfId: Number(shelfId)
  }, { withToken: true })
}

export function findShelfId(buildingId, shelfCode, floor, zone) {
  return http.get('/bookshelf/find-shelf-id', {
    params: { buildingId, shelfCode, floor, zone },
    withToken: true
  })
}

export function borrowBook(bookId) {
  return http.post('/bookshelf/borrow-book', { bookId }, { withToken: true })
}

// ---------- 状态流转：按 BookID ----------
export function borrowBookById(bookId) {
  return http.patch(`/Book/${bookId}/borrow`, null, { withToken: true })
}

export function offShelfBookById(bookId) {
  return http.patch(`/Book/${bookId}/off-shelf`, null, { withToken: true })
}

export function returnBookById(bookId) {
  return http.patch(`/Book/${bookId}/return`, null, { withToken: true })
}

export function onShelfBookById(bookId) {
  return http.patch(`/Book/${bookId}/on-shelf`, null, { withToken: true })
}

// ---------- 状态流转：按条码 ----------
export function borrowBookByBarcode(barcode) {
  return http.patch(`/Book/by-barcode/${encodeURIComponent(barcode)}/borrow`, null, { withToken: true })
}

export function offShelfBookByBarcode(barcode) {
  return http.patch(`/Book/by-barcode/${encodeURIComponent(barcode)}/off-shelf`, null, { withToken: true })
}

export function returnBookByBarcode(barcode) {
  return http.patch(`/Book/by-barcode/${encodeURIComponent(barcode)}/return`, null, { withToken: true })
}

export function onShelfBookByBarcode(barcode) {
  return http.patch(`/Book/by-barcode/${encodeURIComponent(barcode)}/on-shelf`, null, { withToken: true })
}
