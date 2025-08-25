import http from '@/services/http'

export const getReaders = (params) => http.get('/reader/list', { params })
export const getReaderById = (id) => http.get(`/reader/${id}`)
export const addReader = (data) => http.post('/reader', data)
export const updateReader = (data) => http.put('/reader', data)
export const deleteReader = (id) => http.delete(`/reader/${id}`)
export const resetPassword = (id) => http.put(`/reader/${id}/reset-password`)
export const getMyProfile = () => http.get('/reader/me')
export const updateMyProfile = (data) => http.put('/reader/me', data)

export const login = (data) => http.post('/login', data)
export const logout = () => http.post('/logout')
export const getCaptcha = () => http.get('/captcha')

export const register = (data) => http.post('/register', data)

export const getBorrowingRecords = (params) => http.get('/borrowing', { params });
export const getBorrowingRecordById = (id) => http.get(`/borrowing/${id}`);
export const addBorrowingRecord = (data) => http.post('/borrowing', data);
export const updateBorrowingRecord = (data) => http.put('/borrowing', data);
export const deleteBorrowingRecord = (id) => http.delete(`/borrowing/${id}`);
export const returnBook = (id) => http.put(`/borrowing/${id}/return`);
export const renewBorrowing = (id) => http.put(`/borrowing/${id}/renew`);