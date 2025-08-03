// 文件: backend/Repositories/Admin/AnnouncementRepository.cs
// 这是最终修正后的完整代码

using Dapper; // 确保 Dapper 已被 using
using library_system.DTOs.Admin;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data; // 需要这个来使用 ParameterDirection

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

        public async Task<IEnumerable<AnnouncementDto>> GetPubliclyVisibleAsync()
        {
            var sql = "SELECT * FROM announcement_view WHERE Status = '发布中' AND TargetGroup = '所有人' ORDER BY CreateTime DESC";
            using var connection = new OracleConnection(_connectionString);
            return await connection.QueryAsync<AnnouncementDto>(sql);
        }
        
        public async Task<int> CreateAsync(CreateOrUpdateAnnouncementDto dto)
        {
            var sql = @"
                INSERT INTO Announcement (AnnouncementID, Title, Content, TargetGroup, Status, LibrarianID)
                VALUES (SEQ_ANNOUNCEMENT.NEXTVAL, :Title, :Content, :TargetGroup, :Status, :LibrarianID)
                RETURNING AnnouncementID INTO :id";
            
            using var connection = new OracleConnection(_connectionString);
            
            // 【修正】使用标准的 DynamicParameters
            var parameters = new DynamicParameters();
            parameters.Add("Title", dto.Title);
            parameters.Add("Content", dto.Content);
            parameters.Add("TargetGroup", dto.TargetGroup);
            parameters.Add("Status", dto.Status);
            parameters.Add("LibrarianID", dto.LibrarianID);
            // 【修正】定义出参
            parameters.Add("id", dbType: DbType.Int32, direction: ParameterDirection.Output);
            
            await connection.ExecuteAsync(sql, parameters);
            
            // 【修正】获取出参的值
            return parameters.Get<int>("id");
        }

                // 【新增】更新公告的方法
        public async Task<bool> UpdateAsync(int id, CreateOrUpdateAnnouncementDto dto)
        {
            // 定义 SQL 更新语句
            // 根据公告ID (AnnouncementID) 来定位要更新的行
            // 更新标题、内容、目标群体和状态
            var sql = @"
                UPDATE Announcement 
                SET 
                    Title = :Title, 
                    Content = :Content, 
                    TargetGroup = :TargetGroup, 
                    Status = :Status
                WHERE AnnouncementID = :id";
            
            using var connection = new OracleConnection(_connectionString);

            // 将传入的 dto 和 id 组合成一个参数对象传递给 Dapper
            var parameters = new DynamicParameters(dto);
            parameters.Add("id", id);
            
            // ExecuteAsync 会返回受影响的行数
            var affectedRows = await connection.ExecuteAsync(sql, parameters);
            
            // 如果受影响的行数大于0，说明更新成功，返回 true
            return affectedRows > 0;
        }
        
                // 【新增】删除公告的方法
        public async Task<bool> DeleteAsync(int id)
        {
            // 定义 SQL 删除语句，通过公告ID来定位
            var sql = "DELETE FROM Announcement WHERE AnnouncementID = :id";
            
            using var connection = new OracleConnection(_connectionString);
            
            // ExecuteAsync 会返回受影响的行数
            var affectedRows = await connection.ExecuteAsync(sql, new { id });
            
            // 如果受影响的行数大于0，说明删除成功，返回 true
            return affectedRows > 0;
        }

    }
}