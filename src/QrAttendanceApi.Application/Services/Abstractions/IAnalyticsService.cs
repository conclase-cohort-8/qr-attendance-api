using QrAttendanceApi.Application.DTOs;
using QrAttendanceApi.Application.Responses;

namespace QrAttendanceApi.Application.Services.Abstractions
{
    public interface IAnalyticsService
    {
        Task<ApiBaseResponse> GetDashboardSummaryAsync(DateTime date);
        Task<ApiBaseResponse> GetDepartmentBreakdownAsync (DateTime date);
        Task<ApiBaseResponse> GetAttendanceTrendsAsync (DateTime from , DateTime to);
    }
}
