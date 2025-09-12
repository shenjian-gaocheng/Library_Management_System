using Dapper;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.Threading.Tasks;
using backend.DTOs.Admin;
using System.Data;

namespace backend.Repositories.Admin
{
    public class AnnouncementRepository
    {
        private readonly string _connectionString;

        public AnnouncementRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<IEnumerable<AnnouncementDto>> GetAllAnnouncementsAsync()
        {
            var sql = "SELECT * FROM Announcement ORDER BY CreateTime DESC";
            using var connection = new OracleConnection(_connectionString);
            return await connection.QueryAsync<AnnouncementDto>(sql);
        }
        
        public async Task<IEnumerable<AnnouncementDto>> GetPublicAnnouncementsAsync()
        {
            // 【核心修正】
            // 1. WHERE 条件包含了 "所有人" 或 "读者"，范围更广
            // 2. ORDER BY CreateTime DESC 确保按时间倒序排列
            // 3. FETCH FIRST 3 ROWS ONLY 确保只返回最多3条记录
            // Console.WriteLine("check1");
            var sql = @"
                SELECT * FROM announcement_view 
                WHERE Status = '发布中' AND (TargetGroup = '所有人' OR TargetGroup = '读者')
                ORDER BY CreateTime DESC
                FETCH FIRST 3 ROWS ONLY";
            // Console.WriteLine("check2");
                
            using var connection = new OracleConnection(_connectionString);
            return await connection.QueryAsync<AnnouncementDto>(sql);
        }

        public async Task<AnnouncementDto> CreateAnnouncementAsync(UpsertAnnouncementDto dto, int librarianId)
        {
            var sql = @"
                INSERT INTO Announcement (LibrarianID, Title, Content, TargetGroup, Priority, Status)
                VALUES (:LibrarianID, :Title, :Content, :TargetGroup, :Priority, '发布中')
                RETURNING AnnouncementID, CreateTime INTO :AnnouncementID, :CreateTime";
            
            using var connection = new OracleConnection(_connectionString);
            var parameters = new DynamicParameters(dto);
            parameters.Add("LibrarianID", librarianId);
            parameters.Add("AnnouncementID", dbType: DbType.Int32, direction: ParameterDirection.Output);
            parameters.Add("CreateTime", dbType: DbType.DateTime, direction: ParameterDirection.Output);

            await connection.ExecuteAsync(sql, parameters);
            
            return new AnnouncementDto {
                AnnouncementID = parameters.Get<int>("AnnouncementID"),
                LibrarianID = librarianId,
                Title = dto.Title,
                Content = dto.Content,
                TargetGroup = dto.TargetGroup,
                Priority = dto.Priority,
                Status = "发布中",
                CreateTime = parameters.Get<DateTime>("CreateTime")
            };
        }

        public async Task<AnnouncementDto> UpdateAnnouncementAsync(int id, UpsertAnnouncementDto dto)
        {
            var sql = @"
                UPDATE Announcement 
                SET Title = :Title, Content = :Content, Priority = :Priority, TargetGroup = :TargetGroup
                WHERE AnnouncementID = :AnnouncementID";

            using var connection = new OracleConnection(_connectionString);
            await connection.ExecuteAsync(sql, new { dto.Title, dto.Content, dto.Priority, dto.TargetGroup, AnnouncementID = id });

            var updatedSql = "SELECT * FROM Announcement WHERE AnnouncementID = :AnnouncementID";
            return await connection.QuerySingleOrDefaultAsync<AnnouncementDto>(updatedSql, new { AnnouncementID = id });
        }

        public async Task<bool> UpdateStatusAsync(int id, string status)
        {
            var sql = "UPDATE Announcement SET Status = :Status WHERE AnnouncementID = :AnnouncementID";
            using var connection = new OracleConnection(_connectionString);
            var affectedRows = await connection.ExecuteAsync(sql, new { Status = status, AnnouncementID = id });
            return affectedRows > 0;
        }
    }
}