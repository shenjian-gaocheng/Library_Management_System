// 文件: backend/Services/Admin/AdminService.cs
// 这是修正后的完整代码

using library_system.DTOs.Admin;
using library_system.Repositories.Admin;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace library_system.Services.Admin
{
    public class AdminService
    {
        private readonly AdminRepository _repository;

        public AdminService(AdminRepository repository)
        {
            _repository = repository;
        }

        // 【修正】移除了这里的 string? keyword 参数，使其与 Repository 匹配
        public Task<IEnumerable<LibrarianDto>> GetAllLibrariansAsync() => _repository.GetAllAsync(); 
        
        public Task<LibrarianDto> CreateLibrarianAsync(CreateLibrarianDto librarianDto) => _repository.CreateAsync(librarianDto);
        public Task<bool> UpdateLibrarianAsync(int id, UpdateLibrarianDto librarianDto) => _repository.UpdateAsync(id, librarianDto);
        public Task<bool> DeleteLibrarianAsync(int id) => _repository.DeleteAsync(id);
    }
}