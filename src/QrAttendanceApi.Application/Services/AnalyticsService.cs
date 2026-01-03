using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using QrAttendanceApi.Application.Abstractions;
using QrAttendanceApi.Application.DTOs;
using QrAttendanceApi.Application.Responses;
using QrAttendanceApi.Application.Services.Abstractions;
using QrAttendanceApi.Domain.Entities;
using QrAttendanceApi.Domain.Enums;

namespace QrAttendanceApi.Application.Services
{
    public class AnalyticsService : IAnalyticsService
    {
        private readonly IRepositoryManager _repository;
        private readonly UserManager<User> _userManager;

        public AnalyticsService(IRepositoryManager repository , UserManager<User> userManager)
        {
            _repository = repository;
            _userManager = userManager;
        }

        public async Task<ApiBaseResponse> GetAttendanceTrendsAsync(DateTime from, DateTime to)
        {
            if (from.Date > to.Date)
                return new BadRequestResponse("From date cannot be greater than To date");

            var attendanceLogs = await _repository.Attendance.Get (a =>
                                                                    a.Timestamp.Date >= from.Date &&
                                                                    a.Timestamp.Date <= to.Date && 
                                                                    a.Status == AttendanceStatus.Present)
                                                                 .ToListAsync();

            var result = attendanceLogs.GroupBy(a => a.Timestamp.Date)
                                        .Select(g => new AttendanceTrendDto
                                        {
                                            Date = g.Key,
                                            Present = g.Count()
                                        })
                                        .OrderBy (x => x.Date)
                                        .ToList();

            return new OkResponse<List<AttendanceTrendDto>>(result);
        }

        public async Task<ApiBaseResponse> GetDashboardSummaryAsync(DateTime date)
        {
            var start = date.Date;
            var end = start.AddDays(1);

            var attendanceQuery = _repository.Attendance.Get(a => a.Timestamp >= start && a.Timestamp < end);

            var totalScans = await attendanceQuery.CountAsync();
            var present = await attendanceQuery.CountAsync(a => a.Status == AttendanceStatus.Present);
            var late = await attendanceQuery.CountAsync(a => a.Status == AttendanceStatus.Late);
            var absent = await attendanceQuery.CountAsync(a => a.Status == AttendanceStatus.Absent);

            var presentAttendances = attendanceQuery.Where(a => a.Status == AttendanceStatus.Present && a.User != null)
                                                    .Select(a => a.User!)
                                                    .Distinct()
                                                    .ToList();

            int staffPresent = 0;
            int studentsPresent = 0;

            foreach (var user in presentAttendances)
            {
                var roles = await _userManager.GetRolesAsync(user);

                if (roles.Contains(Roles.Staff.ToString())) staffPresent++;

                else if (roles.Contains(Roles.Student.ToString())) studentsPresent++;
            }


            var dto = new DashboardSummaryDto
            {
                Date = start,
                TotalScans = totalScans,
                Present = present,
                Late = late,
                Absent = absent,
                StaffPresent = staffPresent,
                StudentsPresent = studentsPresent
            };

            return new OkResponse<DashboardSummaryDto>(dto); 
        }

        public async Task<ApiBaseResponse> GetDepartmentBreakdownAsync(DateTime date)
        {
            var start = date.Date;
            var end = start.AddDays(1);

            var attendanceLogs = await _repository.Attendance.Get(a => a.Timestamp >= start && a.Timestamp < end)
             .Select(a => new
            {
                DepartmentName = a.QrSession!.Department!.Name,
                a.Status
            }).ToListAsync();

            var result = attendanceLogs.GroupBy(x => x.DepartmentName)
                                       .Select (g => new DepartmentAttendanceBreakdownDto
                                       {
                                           Department = g.Key,
                                           Present = g.Count (x => x.Status == AttendanceStatus.Present),
                                           Absent = g.Count(x => x.Status == AttendanceStatus.Absent)
                                       })
                                       .OrderByDescending(x => x.Present)
                                       .ToList();

            return new OkResponse<List<DepartmentAttendanceBreakdownDto>>(result);           
        }
    }
}
