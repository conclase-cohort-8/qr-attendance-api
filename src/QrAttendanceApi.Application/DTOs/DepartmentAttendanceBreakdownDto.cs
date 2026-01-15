namespace QrAttendanceApi.Application.DTOs
{
    public class DepartmentAttendanceBreakdownDto
    {
        public string Department { get; set; } = string.Empty;
        public int Present { get; set; }
        public int Absent { get; set; }
    }
}
