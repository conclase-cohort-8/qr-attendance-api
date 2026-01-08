using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using QrAttendanceApi.Application.Abstractions;
using QrAttendanceApi.Application.Abstractions.Externals;
using QrAttendanceApi.Application.Commands.Attendance;
using QrAttendanceApi.Application.Commands.QRs;
using QrAttendanceApi.Application.Queries;
using QrAttendanceApi.Application.Responses;
using QrAttendanceApi.Application.Services.Abstractions;
using QrAttendanceApi.Domain.Entities;
using QrAttendanceApi.Domain.Enums;
using System.Linq;

namespace QrAttendanceApi.Application.Services
{
    public class AttendanceService : IAttendanceService
    {
        private readonly IRepositoryManager _repository;
        private readonly ITokenService _tokenService;
        private readonly UserManager<User> _userManager;

        public AttendanceService(IRepositoryManager repository,
            ITokenService tokenService, UserManager<User> userManager)
        {
            _repository = repository;
            _tokenService = tokenService;
            _userManager = userManager;
        }

        public async Task<ApiBaseResponse> CreateAdmin(string adminId, CreateAttendanceAdminCommand command)
        {
            var user = await _userManager.FindByIdAsync(command.UserId);
            if (user == null)
            {
                return new NotFoundResponse("User not Found");
            }

            var session = await _repository.QrSession.FirstOrDefault(x => x.Id == command.QrSessionId && !x.IsDeleted);

            if (session == null)
            {
                return new NotFoundResponse("Session not Found");
            }

            var exist = await _repository.Attendance.ExistsAsync(x => x.UserId == command.UserId && x.QrSessionId == command.QrSessionId && !x.IsDeleted);

            if (exist)
            {
                return new ConflictResponse("Attendance alredy exists");
            }

            var log = new AttendanceLog
            {
                UserId = command.UserId,
                QrSessionId = command.QrSessionId,
                Timestamp = command.Timestamp,
                Status = command.Status
            };

            await _repository.Attendance.AddAsync(log);

            var audit = new AuditLog
            {
                UserId = adminId,
                EntityId = log.Id,
                EntityType = nameof(AttendanceLog),
                Action = ActionType.Create,
                Description = command.Reason ?? "Atendance Created by admin",
                Timestamp = DateTime.UtcNow
            };
            await _repository.Audit.AddAsync(audit);

            await _repository.SaveAsync();
            return new OkResponse<string>("Attendance created successfully");
        }

        public async Task<ApiBaseResponse> Delete(string adminId, Guid attendanceId, DeleteAttendanceCommand command)
        {
            var log = await _repository.Attendance.FirstOrDefault(x => x.Id == attendanceId);

            if(log == null)
            {
                return new NotFoundResponse("Attendance record not found");
            }


            log.IsDeleted = true;

            var audit = new AuditLog
            {
                UserId = adminId,
                EntityId = log.Id,
                EntityType = nameof(AttendanceLog),
                Action = ActionType.Delete,
                Description = command.Reason ?? "Attendance deleted by admin",
                Timestamp = DateTime.UtcNow
            };

            await _repository.Audit.AddAsync(audit);

            _repository.Attendance.Update(log);
            await _repository.SaveAsync();

            return new ApiBaseResponse(true, 200, "Attendance record deleted");
        }

        public async Task<ApiBaseResponse> Edit(string adminId, Guid attendanceId, EditAttendanceCommand command)
        {
            var log = await _repository.Attendance.FirstOrDefault(x => x.Id == attendanceId, true);
            if (log == null)
            {
                return new NotFoundResponse("Attendance record not found");
            }

            log.Status = command.Status;
            log.Timestamp = command.Timestamp;

            var audit = new AuditLog
            {
                UserId = adminId,
                EntityId = log.Id,
                EntityType = nameof(AttendanceLog),
                Action = ActionType.Update,
                Description = command.Reason,
                Timestamp = DateTime.UtcNow
            };

            await _repository.Audit.AddAsync(audit);

            _repository.Attendance.Update(log);
            await _repository.SaveAsync();

            return new ApiBaseResponse(true, 200, "Attendance record updated");
        }

        public async Task<ApiBaseResponse> GetHistory(AttendanceHistoryQuery query)
        {
            if (query.PageSize > 200) query.PageSize = 200;
            if (query.PageSize < 5) query.PageSize = 5;

            IQueryable<AttendanceLog> attendanceQuery = _repository.Attendance.Get(x => !x.IsDeleted)
                  .Include(X => X.User)
                  .Include(X => X.QrSession)
                     .ThenInclude(s => s!.Department);

            if (!string.IsNullOrEmpty(query.Search))
            {
                attendanceQuery = attendanceQuery.Where(x => x.User!.FullName.Contains(query.Search));
            }

            if (query.DepartmentId.HasValue)
            {
                attendanceQuery = attendanceQuery.Where(x => x.QrSession!.DepartmentId == query.DepartmentId);
            }

            var totalItems = await attendanceQuery.CountAsync();

            var logs = await attendanceQuery
                .OrderByDescending(x => x.Timestamp)
                .Skip((query.Page - 1) * query.PageSize)
                .Take(query.PageSize)
                .Select(x => new AttendanceLogDto
                {
                    SessionDescription = x.QrSession!.Description,
                    Type = x.QrSession.Type.ToString(),
                    DepartmentName = x.QrSession.Department!.Name,
                    AttendanceTimeStamp = x.Timestamp,
                    Status = x.Status.ToString(),
                    UserFullName = x.User!.FullName,
                }).ToListAsync();

            var pagedData = new PagedResponse<AttendanceLogDto>(logs, totalItems, query.Page, query.PageSize);
            return new OkResponse<PagedResponse<AttendanceLogDto>>(pagedData);
        }

        public async Task<ApiBaseResponse> MarkAttendance(string? userId, Guid sessionId, SessionAttendanceCommand command)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                return new ForbiddenResponse("Access denied");
            }

            if (!_tokenService.TryValidateQrToken(command.QrToken, sessionId, out string reason))
            {
                return new ForbiddenResponse(reason);
            }

            var session = await _repository.QrSession.FirstOrDefault(x => x.Id == sessionId && x.IsActive && !x.IsDeleted);
            if (session == null)
            {
                return new NotFoundResponse("Session not found or is no longer active");
            }

            var alreadyScanned = await _repository.Attendance.ExistsAsync(x => x.QrSessionId == sessionId && x.UserId == userId);
            if (alreadyScanned)
            {
                return new ConflictResponse("You have already marked attendance for this session");
            }

            var currentTime = DateTime.UtcNow;
            var status = currentTime > session.LateAfter
                ?AttendanceStatus.Late
                : AttendanceStatus.Present;

            var attendance = new AttendanceLog
                {
                 UserId = userId!,
                 QrSessionId = sessionId,
                 Timestamp = currentTime,
                 Status = status
            };

            await _repository.Attendance.AddAsync(attendance);
            await _repository.SaveAsync();

            return new OkResponse<string>("Attendance marked successfully");
        }
    }
}