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

        public Task<IEnumerable<LibrarianDto>> GetAllLibrariansAsync() => _repository.GetAllAsync();
        public Task<LibrarianDto> CreateLibrarianAsync(CreateLibrarianDto librarianDto) => _repository.CreateAsync(librarianDto);
        public Task<bool> UpdateLibrarianAsync(string id, UpdateLibrarianDto librarianDto) => _repository.UpdateAsync(id, librarianDto);
        public Task<bool> DeleteLibrarianAsync(string id) => _repository.DeleteAsync(id);
    }
}