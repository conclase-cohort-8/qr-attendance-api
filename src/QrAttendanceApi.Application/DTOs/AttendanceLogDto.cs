public class AttendanceLogDto
{
    public string SessionDescription { get; set; } = default!;
    public string Type { get; set; } = default!;
    public string DepartmentName { get; set; } = default!;  
    public DateTime AttendanceTimeStamp { get; set; }
    public string Status { get; set; } = default!;
    public string UserFullName { get; set; } = default!;
}
