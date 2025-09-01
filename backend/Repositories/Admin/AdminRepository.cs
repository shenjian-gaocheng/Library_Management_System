using Dapper;
using library_system.DTOs.Admin; // 确保引用了 DTO 命名空间
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data;

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
        public async Task<IEnumerable<LibrarianDto>> GetAllAsync() // <-- 确保这里没有参数
        {
            var sql = "SELECT LibrarianID, Name, Permission FROM librarian_view";
            using var connection = new OracleConnection(_connectionString);
            return await connection.QueryAsync<LibrarianDto>(sql);
        }

        // 增
        public async Task<LibrarianDto> CreateAsync(CreateLibrarianDto librarianDto)
        {
            // 【最终修正】从 INSERT 语句中彻底移除 LibrarianID
            var sql = @"
                INSERT INTO Librarian (Name, Password, Permission)
                VALUES (:Name, :Password, :Permission)
                RETURNING LibrarianID INTO :id"; // 依然通过 RETURNING 获取新 ID
                
            using var connection = new OracleConnection(_connectionString);
            
            var parameters = new DynamicParameters();
            parameters.Add("Name", librarianDto.Name);
            parameters.Add("Password", librarianDto.Password);
            parameters.Add("Permission", librarianDto.Permission);
            parameters.Add("id", dbType: DbType.Int32, direction: ParameterDirection.Output);
            
            await connection.ExecuteAsync(sql, parameters);
            
            // 获取数据库自动生成的新 ID
            var newId = parameters.Get<int>("id");
            
            // 返回一个包含了新 ID 的 DTO 对象
            return new LibrarianDto {
                LibrarianID = newId,
                Name = librarianDto.Name,
                Permission = librarianDto.Permission
            };
        }

        // 改
        public async Task<bool> UpdateAsync(int librarianId, UpdateLibrarianDto librarianDto)
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
        public async Task<bool> DeleteAsync(int librarianId)
        {
            var sql = "DELETE FROM Librarian WHERE LibrarianID = :LibrarianID";
            using var connection = new OracleConnection(_connectionString);
            var affectedRows = await connection.ExecuteAsync(sql, new { LibrarianID = librarianId });
            return affectedRows > 0;
        }
    }
}