using library_system.DTOs.Admin;
    // ↓↓↓ 这是最关键的新增行，告诉编译器去哪里找 AnnouncementRepository ↓↓↓
using library_system.Repositories.Admin; 
using System.Collections.Generic;
using System.Threading.Tasks;

namespace library_system.Services.Admin
{
    public class AnnouncementService
    {
        private readonly AnnouncementRepository _repository;
        public AnnouncementService(AnnouncementRepository repository) { _repository = repository; }
        
        public Task<IEnumerable<AnnouncementDto>> GetAllAnnouncementsAsync() => _repository.GetAllAsync();
        public Task<IEnumerable<AnnouncementDto>> GetPublicAnnouncementsAsync() => _repository.GetPubliclyVisibleAsync();
        public Task<int> CreateAnnouncementAsync(CreateOrUpdateAnnouncementDto dto) => _repository.CreateAsync(dto);
        public Task<bool> UpdateAnnouncementAsync(int id, CreateOrUpdateAnnouncementDto dto) => _repository.UpdateAsync(id, dto);
        public Task<bool> DeleteAnnouncementAsync(int id) => _repository.DeleteAsync(id);
    }
}