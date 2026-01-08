namespace QrAttendanceApi.Application.Abstractions
{
    public interface IRepositoryManager
    {
        ITokenRepository Token { get; }
        IDepartmentRepository Department { get; }
        IQrSessionRepository QrSession { get; }
        IAttendanceRepository Attendance { get; }

        IAuditRepository Audit { get; }

        Task SaveAsync(CancellationToken cancellationToken = default!);
    }
}
