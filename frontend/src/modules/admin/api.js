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


// [公开] 获取可发布的公告
export function getPublicAnnouncements() {
  return http.get('/announcements/public');
}

// [管理] 获取所有公告以供管理
export function getAllAnnouncementsForManagement() {
  return http.get('/announcements/manage');
}

// [管理] 创建一个新公告
export function createAnnouncement(data) {
  // data 格式: { title, content, targetGroup, status, librarianID }
  return http.post('/announcements/manage', data);
}

export function deleteAnnouncement(id) {
  // 注意：后端的管理接口，我们之前都统一在了 /manage 路径下
  return http.delete(`/announcements/manage/${id}`);
}

export function updateAnnouncement(id, data) {
  return http.put(`/announcements/manage/${id}`, data);
}