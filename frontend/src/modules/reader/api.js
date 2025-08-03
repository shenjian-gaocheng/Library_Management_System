import http from '@/services/http'



// ——————————————————认证相关接口————————————————
export const login = (data) => http.post('/login', data)
export const logout = () => http.post('/logout',null,{withToken:true})
export const getCaptcha = () => http.get('/captcha')

export const register = (data) => http.post('/register', data)


// ——————————————————Reader相关接口————————————————
export const getReaders = (params) => http.get('/reader/list', { withToken:true,params })//params为可选参数，一般用于分页查询等
export const getReaderById = (id) => http.get(`/reader/${id}`,{withToken:true})
export const addReader = (data) => http.post('/reader', data,{withToken:true})
export const updateReader = (data) => http.put('/reader', data,{withToken:true})
export const deleteReader = (id) => http.delete(`/reader/${id}`,{withToken:true})


export const resetPassword = () => http.put(`/reader/me/resetPwd`,{withToken:true})
export const getMyProfile = () => http.get('/reader/me/info',{withToken:true})
export const getAvatar = (avatarUrl) => http.get(`/reader/me/avatar/${avatarUrl}`,{withToken:true})
export const uploadAvatar = (file) => {
  const formData = new FormData()
  formData.append('file', file)

  return http.post('/reader/me/upload/avatar', formData, {
    headers: {
      'Content-Type': 'multipart/form-data'
    },
    withToken:true
  })
}

export const updateAvatar = (avatarUrl) =>
  http.put(`/reader/me/avatar?avatarUrl=${encodeURIComponent(avatarUrl)}`, null, { withToken: true })

export const updateMyProfile = (data) => http.put('/reader/me/info', data,{withToken:true})
