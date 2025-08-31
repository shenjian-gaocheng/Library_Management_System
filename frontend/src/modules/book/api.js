
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




export function getCommentsByISBN(ISBN) {
  return http.get('/comment/search', {
    params: { ISBN }
  })
}

export function getCommentsByID(id) {
  return http.get('/comment/search-id', {
    params: { id }
  })
}
export function addComment(commentData) {
  return http.post('/comment/add', commentData)
}


export function getReportsByReaderId(readerId) {
  return http.get(`/report/by-reader/${readerId}`)
}

export function getReportsByLibrarianId(librarianId) {
  return http.get(`/report/by-librarian/${librarianId}`)
}

export function addReport(reportData) {
  return http.post('/report/add', reportData)
}



export function changeReportStatus(statusData) {
  return http.post('/report/change-status', statusData)
}

export function getCategoryTree() {
  return http.get('/api/Category/tree')
}
export function addCategory(data) {
  return http.post('/api/Category', data)
}
export function updateCategory(data) {
  return http.put('/api/Category', data)
}
export function deleteCategory(id, operatorId) {
  return http.delete(`/api/Category/${id}?operatorId=${operatorId}`)
}

export function getBooksBookShelf(keyword) {
return http.get('/bookshelf/search_book_which_shelf', {
    params: { keyword }
})
}

export function getShelf(keyword) {
return http.get('/bookshelf/search_bookshelf', {
    params: { keyword }
})
}
export function addShelf(buildingid, shelfcode, floor, zone) {
  return http.post('/bookshelf/add_bookshelf', {
    buildingid: Number(buildingid),
    shelfcode: shelfcode,
    floor: Number(floor),
    zone: zone
  })
}

export function deleteShelf(shelfId) {
  return http.delete(`/bookshelf/delete/${shelfId}`);
}

export function checkShelfHasBooks(shelfId) {
  return http.get(`/bookshelf/has-books/${shelfId}`);
}

// 检查书架是否存在
export function checkShelfExists(buildingId, shelfCode, floor, zone) {
  return http.get('/bookshelf/check-shelf-exists', {
    params: { buildingId, shelfCode, floor, zone }
  });
}

export function returnBook(bookId, shelfId) {
  return http.post('/bookshelf/return-book', { 
    bookId: Number(bookId),
    shelfId: Number(shelfId)
  });
}
// 获取书架ID
export function findShelfId(buildingId, shelfCode, floor, zone) {
  return http.get('/bookshelf/find-shelf-id', {
    params: { 
      buildingId, 
      shelfCode, 
      floor, 
      zone 
    }
  });
}

export function borrowBook(bookId) {
  return http.post('/bookshelf/borrow-book', { bookId })
}

// ---------- 查询单册 ----------
export function getBookById(bookId) {
  return http.get(`/Book/${bookId}`)
}

export function getBookByBarcode(barcode) {
  return http.get(`/Book/by-barcode/${encodeURIComponent(barcode)}`)
}

// ---------- 状态流转：按 BookID ----------
export function borrowBookById(bookId) {
  return http.patch(`/Book/${bookId}/borrow`)
}

export function offShelfBookById(bookId) {
  return http.patch(`/Book/${bookId}/off-shelf`)
}

export function returnBookById(bookId) {
  return http.patch(`/Book/${bookId}/return`)
}

export function onShelfBookById(bookId) {
  return http.patch(`/Book/${bookId}/on-shelf`)
}

// ---------- 状态流转：按条码 ----------
export function borrowBookByBarcode(barcode) {
  return http.patch(`/Book/by-barcode/${encodeURIComponent(barcode)}/borrow`)
}

export function offShelfBookByBarcode(barcode) {
  return http.patch(`/Book/by-barcode/${encodeURIComponent(barcode)}/off-shelf`)
}

export function returnBookByBarcode(barcode) {
  return http.patch(`/Book/by-barcode/${encodeURIComponent(barcode)}/return`)
}

export function onShelfBookByBarcode(barcode) {
  return http.patch(`/Book/by-barcode/${encodeURIComponent(barcode)}/on-shelf`)
}