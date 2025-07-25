// 文件: frontend/src/modules/admin/api.js
import http from '@/services/http.js';

// 获取所有管理员列表
export function getLibrarians() {
  return http.get('/admin');
}

// 新增一个管理员
export function createLibrarian(data) {
  // data 的格式应为 { librarianID, name, password, permission }
  return http.post('/admin', data);
}

// 更新一个管理员信息
export function updateLibrarian(id, data) {
  // data 的格式应为 { name, permission }
  return http.put(`/admin/${id}`, data);
}

// 删除一个管理员
export function deleteLibrarian(id) {
  return http.delete(`/admin/${id}`);
}