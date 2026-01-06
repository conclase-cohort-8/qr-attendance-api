
namespace QrAttendanceApi.Application.Abstractions
{
    public interface IUserRoleRepository
    {
        Task<List<(string UserId, string? RoleName)>> GetUserRoles(List<string> userIds);
    }
}