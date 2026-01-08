using QrAttendanceApi.Application.Abstractions;
using QrAttendanceApi.Infrastructure.Persistence;

namespace QrAttendanceApi.Infrastructure.Repositories
{
    public class RepositoryManager : IRepositoryManager
    {
        private readonly AppDbContext _dbContext;
        private readonly Lazy<ITokenRepository> _tokenRepository;
        private readonly Lazy<IDepartmentRepository> _departmentRepository;
        private readonly Lazy<IQrSessionRepository> _qrSessionRepository;
        private readonly Lazy<IAttendanceRepository> _attendanceRepository;
        private readonly Lazy<IAuditRepository> _auditRepository;


        public RepositoryManager(AppDbContext dbContext)
        {
            _dbContext = dbContext;

            _tokenRepository = new Lazy<ITokenRepository>(()
                => new TokenRepository(dbContext));
            _departmentRepository = new Lazy<IDepartmentRepository>(()
                => new DepartmentRepository(dbContext));
            _qrSessionRepository = new Lazy<IQrSessionRepository>(() 
                => new QrSessionRepository(dbContext));
            _attendanceRepository = new Lazy<IAttendanceRepository>(() 
                => new AttendanceRepository(dbContext));

            _auditRepository = new Lazy<IAuditRepository>(()
                => new AuditRepository(dbContext));
        }
        public ITokenRepository Token => _tokenRepository.Value;
        public IDepartmentRepository Department => _departmentRepository.Value;
        public IQrSessionRepository QrSession => _qrSessionRepository.Value;
        public IAttendanceRepository Attendance => _attendanceRepository.Value;

        public IAuditRepository Audit => _auditRepository.Value;

        public async Task SaveAsync(CancellationToken cancellationToken = default)
        {
           await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
