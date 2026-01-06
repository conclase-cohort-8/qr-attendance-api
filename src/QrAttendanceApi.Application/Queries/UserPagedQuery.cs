namespace QrAttendanceApi.Application.Queries
{
    public class UserPagedQuery : PageQuery
    {
        public string? Search { get; set; }
        public Guid? DepartmentId { get; set; }
    }
}