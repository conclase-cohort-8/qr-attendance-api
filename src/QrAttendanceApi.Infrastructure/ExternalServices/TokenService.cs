using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using QrAttendanceApi.Application.Abstractions;
using QrAttendanceApi.Application.Abstractions.Externals;
using QrAttendanceApi.Application.Settings;
using QrAttendanceApi.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace QrAttendanceApi.Infrastructure.ExternalServices
{
    public class TokenService : ITokenService
    {
        private readonly JwtSecurityTokenHandler _handler = new();
        private readonly JwtSettings _settings;
        private readonly byte[] _key;
        private readonly IRepositoryManager _repository;

        public TokenService(IOptions<JwtSettings> options,
                            IRepositoryManager repository)
        {
            var settings = options.Value ?? 
                throw new ArgumentNullException(nameof(JwtSettings));
            _key = Encoding.UTF8.GetBytes(settings.QrSecret);
            _settings = settings;
            _repository = repository;
        }

        public string GenerateQrToken(Guid sessionId, int ttlSeconds = 60)
        {
            throw new NotImplementedException();
        }

        public bool TryValidateQrToken(string token, out string reason)
        {
            throw new NotImplementedException();
        }

        public string CreateAccessToken(User user, string[] roles)
        {
            var claims = GetClaims(user, roles);
            var creds = GetSigningCredentials();
            var token = GetSecurityToken(claims, creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<string> CreateAndSaveRefreshToken(string userId)
        {
            var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
            var hash = ComputeHash(token);

            await _repository.Token.AddAsync(new RefreshToken
            {
                Hash = hash,
                UserId = userId,
                ExpiresAt = DateTimeOffset.UtcNow.Add(TimeSpan.FromDays(_settings.Expires))
            });

            await _repository.SaveAsync();
            return token;
        }

        public string ComputeHash(string token)
        {
            using var sha = SHA256.Create();
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(token));
            return Convert.ToBase64String(bytes);
        }

        #region Private Methods
        private List<Claim> GetClaims(User user, string[] roles)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName!),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
            };

            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            return claims;
        }

        private SigningCredentials GetSigningCredentials()
        {
            var key = Encoding.UTF8.GetBytes(_settings.Key);
            var secret = new SymmetricSecurityKey(key);
            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }

        private JwtSecurityToken GetSecurityToken(List<Claim> claims, SigningCredentials credentials)
        {
            return new JwtSecurityToken(
                issuer: _settings.Issuer,
                audience: _settings.Audience,
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddHours(_settings.Expires),
                signingCredentials: credentials
            );
        }
        #endregion
    }
}