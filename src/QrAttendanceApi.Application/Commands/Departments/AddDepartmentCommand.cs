namespace QrAttendanceApi.Application.Commands.Departments
{
    public class AddDepartmentCommand
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}
