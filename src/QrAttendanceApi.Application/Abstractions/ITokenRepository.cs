using QrAttendanceApi.Domain.Entities;
using System.Linq.Expressions;

namespace QrAttendanceApi.Application.Abstractions
{
    public interface ITokenRepository
    {
        Task AddAsync(RefreshToken token, bool save = true);
        Task<RefreshToken?> FindAsync(Expression<Func<RefreshToken, bool>> condition);
        Task UpdateAsync(RefreshToken token, bool save = true);
    }
}
