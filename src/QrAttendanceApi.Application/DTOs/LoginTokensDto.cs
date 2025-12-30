namespace QrAttendanceApi.Application.DTOs
{
    public record LoginTokensDto(string AccessToken, string RefreshToken);
    public record QrTokenDto(string Token, int ExpiresInSeconds);
}
