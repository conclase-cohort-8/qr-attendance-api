namespace QrAttendanceApi.Application.Abstractions
{
    public interface IRepositoryManager
    {
        ITokenRepository Token { get; }
        IDepartmentRepository Department { get; }
        IQrSessionRepository QrSession { get; }
        IAttendanceRepository Attendance { get; }

        Task SaveAsync(CancellationToken cancellationToken = default!);
    }
}
