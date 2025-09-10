// 文件: backend/Repositories/Admin/AnnouncementRepository.cs
// 这是最终的、包含了“获取最新3条”逻辑的完整版本

using Dapper;
using library_system.DTOs.Admin;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data;

namespace library_system.Repositories.Admin
{
    public class AnnouncementRepository
    {
        private readonly string _connectionString;

        public AnnouncementRepository(string connectionString) 
        {
            _connectionString = connectionString;
        }

        public async Task<IEnumerable<AnnouncementDto>> GetAllAsync()
        {
            var sql = "SELECT * FROM announcement_view ORDER BY CreateTime DESC";
            using var connection = new OracleConnection(_connectionString);
            return await connection.QueryAsync<AnnouncementDto>(sql);
        }

        // 公开用：只获取正在发布的、面向所有人的、最新的三条公告
        public async Task<IEnumerable<AnnouncementDto>> GetPubliclyVisibleAsync()
        {
            // 【核心修正】
            // 1. WHERE 条件包含了 "所有人" 或 "读者"，范围更广
            // 2. ORDER BY CreateTime DESC 确保按时间倒序排列
            // 3. FETCH FIRST 3 ROWS ONLY 确保只返回最多3条记录
            Console.WriteLine("check1");
            var sql = @"
                SELECT * FROM announcement_view 
                WHERE Status = '发布中' AND (TargetGroup = '所有人' OR TargetGroup = '读者')
                ORDER BY CreateTime DESC
                FETCH FIRST 3 ROWS ONLY";
            Console.WriteLine("check2");
                
            using var connection = new OracleConnection(_connectionString);
            return await connection.QueryAsync<AnnouncementDto>(sql);
        }
        

        // 创建公告
        public async Task<int> CreateAsync(CreateOrUpdateAnnouncementDto dto)
        {
            using var connection = new OracleConnection(_connectionString);
            await connection.OpenAsync();

            // ===================================================================
            // 【终极诊断：在 INSERT 之前，先执行一次 SELECT 查询】
            // 我们要让应用程序自己去确认，它到底能不能看到那个父项关键字
            // ===================================================================
            try
            {
                var validationSql = "SELECT COUNT(*) FROM LIBRARIAN WHERE LIBRARIANID = :LibrarianID";
                Console.WriteLine("===================== [终极诊断开始] =====================");
                Console.WriteLine($"[诊断] 准备验证 LibrarianID: {dto.LibrarianID} 是否存在...");
                
                var count = await connection.QuerySingleAsync<int>(validationSql, new { dto.LibrarianID });
                
                Console.WriteLine($"[诊断] 查询 LIBRARIAN 表完成。对于 ID = {dto.LibrarianID}，找到了 {count} 条记录。");

                if (count == 0)
                {
                    // 如果程序自己都找不到，那就证明确实有问题
                    Console.WriteLine("[诊断] 致命错误：应用程序在自己的连接会话中，找不到指定的 LibrarianID！");
                    Console.WriteLine("===================== [终极诊断结束 - 失败] =====================");
                    throw new Exception($"[DIAGNOSTIC FAILURE] The database reports that NO librarian exists with ID = {dto.LibrarianID}. Please verify the data and that the transaction in SQL Developer was truly COMMITTED.");
                }
                Console.WriteLine("[诊断] 验证通过！应用程序可以看到父项关键字。现在尝试插入...");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[诊断] 在验证阶段就发生了异常: {ex.Message}");
                Console.WriteLine("===================== [终极诊断结束 - 异常] =====================");
                throw; // 重新抛出异常
            }
            // ===================================================================
            // 诊断结束
            // ===================================================================

            // 如果验证通过，才继续执行插入操作
            var insertSql = @"
                INSERT INTO Announcement (Title, Content, TargetGroup, Status, LibrarianID)
                VALUES (:Title, :Content, :TargetGroup, :Status, :LibrarianID)
                RETURNING AnnouncementID INTO :id";
            
            var parameters = new DynamicParameters(dto);
            parameters.Add("id", dbType: DbType.Int32, direction: ParameterDirection.Output);
            
            await connection.ExecuteAsync(insertSql, parameters);
            
            return parameters.Get<int>("id");
        }


        public async Task<bool> UpdateAsync(int id, CreateOrUpdateAnnouncementDto dto)
        {
            var sql = @"
                UPDATE Announcement 
                SET 
                    Title = :Title, 
                    Content = :Content, 
                    TargetGroup = :TargetGroup, 
                    Status = :Status
                WHERE AnnouncementID = :id";
            
            using var connection = new OracleConnection(_connectionString);
            var parameters = new DynamicParameters(dto);
            parameters.Add("id", id);
            var affectedRows = await connection.ExecuteAsync(sql, parameters);
            return affectedRows > 0;
        }
        
        public async Task<bool> DeleteAsync(int id)
        {
            var sql = "DELETE FROM Announcement WHERE AnnouncementID = :id";
            using var connection = new OracleConnection(_connectionString);
            var affectedRows = await connection.ExecuteAsync(sql, new { id });
            return affectedRows > 0;
        }
    }
}