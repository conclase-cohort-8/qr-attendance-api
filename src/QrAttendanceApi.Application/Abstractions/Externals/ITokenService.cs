using QrAttendanceApi.Domain.Entities;

namespace QrAttendanceApi.Application.Abstractions.Externals
{
    public interface ITokenService
    {
        string ComputeHash(string token);
        string CreateAccessToken(User user, string[] roles);
        Task<string> CreateAndSaveRefreshToken(string userId);
        string GenerateQrToken(Guid sessionId, int ttlSeconds = 60);
        bool TryValidateQrToken(string token, out string reason);
    }
}
