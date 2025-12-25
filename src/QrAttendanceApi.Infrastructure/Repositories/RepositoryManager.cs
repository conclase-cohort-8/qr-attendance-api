using QrAttendanceApi.Application.Abstractions;
using QrAttendanceApi.Infrastructure.Persistence;

namespace QrAttendanceApi.Infrastructure.Repositories
{
    public class RepositoryManager : IRepositoryManager
    {
        private readonly AppDbContext _dbContext;
        private readonly Lazy<ITokenRepository> _tokenRepository;
        private readonly Lazy<IDepartmentRepository> _departmentRepository;

        public RepositoryManager(AppDbContext dbContext)
        {
            _dbContext = dbContext;

            _tokenRepository = new Lazy<ITokenRepository>(()
                => new TokenRepository(dbContext));
            _departmentRepository = new Lazy<IDepartmentRepository>(()
                => new DepartmentRepository(dbContext));
        }
        public ITokenRepository Token => _tokenRepository.Value;
        public IDepartmentRepository Department => _departmentRepository.Value;

        public async Task SaveAsync(CancellationToken cancellationToken = default)
        {
           await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
