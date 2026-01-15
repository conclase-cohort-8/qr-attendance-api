namespace QrAttendanceApi.Application.DTOs
{
    public class DashboardSummaryDto
    {
        public DateTime Date { get; set; }
        public int TotalScans { get; set; }
        public int Present { get; set; }
        public int Absent { get; set; }
        public int Late { get; set; }
        public int StaffPresent { get; set; }
        public int StudentsPresent { get; set; }
    }

}
