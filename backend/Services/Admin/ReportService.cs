using System.Collections.Generic;
using System.Threading.Tasks;
using backend.DTOs.Admin;
using backend.Repositories.Admin;
using System;
using System.Linq;

namespace backend.Services.Admin
{
    public class ReportService
    {
        private readonly ReportRepository _repository;

        public ReportService(ReportRepository repository)
        {
            _repository = repository;
        }

        public Task<IEnumerable<ReportDetailDto>> GetPendingReportsAsync()
        {
            return _repository.GetPendingReportsAsync();
        }

        public async Task<bool> HandleReportAsync(int reportId, string action, bool banUser, int adminId)
        {
            var pendingReports = await _repository.GetPendingReportsAsync();
            var report = pendingReports.FirstOrDefault(r => r.ReportID == reportId);

            if (report == null)
            {
                throw new InvalidOperationException("Report not found or has already been handled.");
            }

            return action.ToLower() switch
            {
                "approve" => await _repository.HandleReportTransactionAsync(reportId, report.CommenterID, report.CommentID, "处理完成", "已删除", banUser, adminId),
                "reject" => await _repository.HandleReportTransactionAsync(reportId, report.CommenterID, report.CommentID, "驳回", null, false, adminId), // Rejecting never bans
                _ => throw new ArgumentException("Invalid action specified.")
            };
        }
        
        public async Task<int> AddReportAsync(ReportDto report)
        {
            return await _repository.AddReportAsync(report);
        }
    }
}