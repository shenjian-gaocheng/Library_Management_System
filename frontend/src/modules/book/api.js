import http from '@/services/http.js' 

export function getBooks(keyword) {
return http.get('/book/search', {
    params: { keyword }
})
}

export function getBooksBookShelf(keyword) {
return http.get('/book/search_book_which_shelf', {
    params: { keyword }
})
}