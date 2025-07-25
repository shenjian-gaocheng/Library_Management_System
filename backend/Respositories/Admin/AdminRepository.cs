using Dapper;
using library_system.DTOs.Admin; // 确保引用了 DTO 命名空间
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace library_system.Repositories.Admin
{
    public class AdminRepository
    {
        private readonly string _connectionString;

        public AdminRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        // 查 (所有)
        public async Task<IEnumerable<LibrarianDto>> GetAllAsync()
        {
            var sql = "SELECT LibrarianID, Name, Permission FROM librarian_view"; // 从视图查询
            using var connection = new OracleConnection(_connectionString);
            return await connection.QueryAsync<LibrarianDto>(sql);
        }

        // 增
        public async Task<LibrarianDto> CreateAsync(CreateLibrarianDto librarianDto)
        {
            // !!! 安全警告: 您的表结构直接存储明文密码，这是极不安全的。
            // 在生产项目中，必须使用 BCrypt.Net 等库将密码哈希化后再存储。
            // string hashedPassword = BCrypt.Net.BCrypt.HashPassword(librarianDto.Password);
            
            var sql = @"
                INSERT INTO Librarian (LibrarianID, Name, Password, Permission)
                VALUES (:LibrarianID, :Name, :Password, :Permission)";
                
            using var connection = new OracleConnection(_connectionString);
            await connection.ExecuteAsync(sql, librarianDto);

            return new LibrarianDto {
                LibrarianID = librarianDto.LibrarianID,
                Name = librarianDto.Name,
                Permission = librarianDto.Permission
            };
        }

        // 改
        public async Task<bool> UpdateAsync(string librarianId, UpdateLibrarianDto librarianDto)
        {
            var sql = "UPDATE Librarian SET Name = :Name, Permission = :Permission WHERE LibrarianID = :LibrarianID";
            using var connection = new OracleConnection(_connectionString);
            var affectedRows = await connection.ExecuteAsync(sql, new { 
                Name = librarianDto.Name, 
                Permission = librarianDto.Permission, 
                LibrarianID = librarianId 
            });
            return affectedRows > 0;
        }

        // 删
        public async Task<bool> DeleteAsync(string librarianId)
        {
            var sql = "DELETE FROM Librarian WHERE LibrarianID = :LibrarianID";
            using var connection = new OracleConnection(_connectionString);
            var affectedRows = await connection.ExecuteAsync(sql, new { LibrarianID = librarianId });
            return affectedRows > 0;
        }
    }
}