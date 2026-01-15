namespace QrAttendanceApi.Application.Services.Abstractions
{
    public interface IServiceManager
    {
        IAccountService Account {  get; }
        IDepartmentService Department { get; }
        IQrSessionService QrSession { get; }
        IAttendanceService Attendance { get; }
        IAnalyticsService Analytics { get; }
    }
}