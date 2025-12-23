namespace QrAttendanceApi.Application.Abstractions
{
    public interface IRepositoryManager
    {
        ITokenRepository Token { get; }
        IDepartmentRepository Department { get; }

        Task SaveAsync(CancellationToken cancellationToken = default!);
    }
}
