using QrAttendanceApi.Domain.Entities;

namespace QrAttendanceApi.Application.Abstractions
{
    public interface IDepartmentRepository
    {
        Task AddAsync(Department department, bool save = true);
        Task<List<Department>> GetAll();
    }
}
