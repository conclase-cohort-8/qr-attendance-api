using QrAttendanceApi.Application.Commands.Departments;
using QrAttendanceApi.Application.Responses;

namespace QrAttendanceApi.Application.Services.Abstractions
{
    public interface IDepartmentService
    {
        Task<ApiBaseResponse> CreateAsync(AddDepartmentCommand command);
        Task<ApiBaseResponse> GetDepartments();
    }
}
