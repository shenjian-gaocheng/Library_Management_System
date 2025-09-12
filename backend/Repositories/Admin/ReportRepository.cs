using Dapper;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.Threading.Tasks;
using backend.DTOs.Admin;

namespace backend.Repositories.Admin
{
    public class ReportRepository
    {
        private readonly string _connectionString;

        public ReportRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<IEnumerable<ReportDetailDto>> GetPendingReportsAsync()
        {
            var sql = "SELECT * FROM V_ReportDetails_Enhanced WHERE ReportStatus = '待处理' ORDER BY ReportTime ASC";
            using var connection = new OracleConnection(_connectionString);
            return await connection.QueryAsync<ReportDetailDto>(sql);
        }

        public async Task<bool> HandleReportTransactionAsync(int reportId, int commenterId, int commentId, string newReportStatus, string? newCommentStatus, bool banUser, int librarianId)
        {
            using var connection = new OracleConnection(_connectionString);
            await connection.OpenAsync();
            using var transaction = connection.BeginTransaction();

            try
            {
                // 1. 更新举报记录的状态
                var reportSql = "UPDATE Report SET Status = :Status, LibrarianID = :LibrarianID WHERE ReportID = :ReportID";
                var reportResult = await connection.ExecuteAsync(reportSql, new { Status = newReportStatus, LibrarianID = librarianId, ReportID = reportId }, transaction);

                // 2. 如果需要，更新评论的状态
                if (!string.IsNullOrEmpty(newCommentStatus))
                {
                    var commentSql = "UPDATE Comment_Table SET Status = :Status WHERE CommentID = :CommentID";
                    await connection.ExecuteAsync(commentSql, new { Status = newCommentStatus, CommentID = commentId }, transaction);
                }

                // 3. 如果需要，禁言用户
                if (banUser)
                {
                    var readerSql = "UPDATE Reader SET AccountStatus = '冻结' WHERE ReaderID = :ReaderID";
                    await connection.ExecuteAsync(readerSql, new { ReaderID = commenterId }, transaction);
                }

                transaction.Commit();
                return reportResult > 0;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }
    }
}