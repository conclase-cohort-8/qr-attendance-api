//using QrAttendanceApi.Domain.Entities;

//namespace QrAttendanceApi.Application.Abstractions
//{
//    public interface IAttendanceRepository : IRepository<AttendanceLog>
//    {
//        // Check if a user has already scanned a session
//        Task<bool> HasUserScannedAsync(Guid sessionId, Guid userId);

//        // Get paginated attendance history
//        Task<(List<AttendanceLog> Data, int TotalCount)> GetHistoryAsync(
//            int page, int pageSize, Guid? departmentId = null, string? search = null,
//            DateTime? from = null, DateTime? to = null);

//        // Get all attendance logs for a session
//        Task<List<AttendanceLog>> GetBySessionIdAsync(Guid sessionId);

//        // Soft delete attendance log
//        Task SoftDeleteAsync(AttendanceLog attendanceLog);
//    }
//}


using QrAttendanceApi.Domain.Entities;

namespace QrAttendanceApi.Application.Abstractions
{
    public interface IAttendanceRepository : IRepository<AttendanceLog>
    {
        Task<bool> HasUserScannedAsync(Guid sessionId, Guid userId);

        Task<(List<AttendanceLog> Data, int TotalCount)> GetHistoryAsync(
            int page,
            int pageSize,
            Guid? departmentId = null,
            string? search = null,
            DateTime? from = null,
            DateTime? to = null);

        Task<List<AttendanceLog>> GetBySessionIdAsync(Guid sessionId);

        Task SoftDeleteAsync(AttendanceLog attendanceLog);
    }
}
