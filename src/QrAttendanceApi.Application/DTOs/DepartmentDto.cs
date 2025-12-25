using QrAttendanceApi.Domain.Entities;

namespace QrAttendanceApi.Application.DTOs
{
    public class DepartmentDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public static DepartmentDto ToDto(Department department)
        {
            return new DepartmentDto
            {
                Id = department.Id,
                Name = department.Name,
            };
        }
    }
}
