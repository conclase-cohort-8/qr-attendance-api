using DocumentFormat.OpenXml.Drawing;
using Microsoft.AspNetCore.Identity;
using QrAttendanceApi.Application.Abstractions;
using QrAttendanceApi.Application.Abstractions.Externals;
using QrAttendanceApi.Application.Commands.QRs;
using QrAttendanceApi.Application.DTOs;
using QrAttendanceApi.Application.Responses;
using QrAttendanceApi.Application.Services.Abstractions;
using QrAttendanceApi.Application.Validations;
using QrAttendanceApi.Domain.Entities;
using QrAttendanceApi.Domain.Enums;
using System;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace QrAttendanceApi.Application.Services
{
    public class QrSessionService : IQrSessionService
    {
        private readonly IRepositoryManager _repository;
        private readonly UserManager<User> _userManager;
        private readonly ITokenService _tokenService;

        public QrSessionService(IRepositoryManager manager,
                                UserManager<User> userManager,
                                ITokenService tokenService)
        {
            _repository = manager;
            _userManager = userManager;
            _tokenService = tokenService;
        }

        public async Task<ApiBaseResponse> Create(string? userId, CreateQrSessionCommand command)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                return new ForbiddenResponse("access denied");
            }
            var validator = new CreateQrSessionCommandValidator().Validate(command);
            if (!validator.IsValid)
            {
                return new BadRequestResponse(validator.Errors?.FirstOrDefault()?.ErrorMessage ?? "Invalid imput");
            }

            var loggedInUser = await _userManager.FindByIdAsync(userId);
            if (loggedInUser == null)
            {
                return new ForbiddenResponse("User not found");
            }

            var department = await _repository.Department.FirstOrDefault(d => d.Id == command.DepartmentId);
            if (department == null)
            {
                return new NotFoundResponse(" selected department not found");
            }


            var session = CreateQrSessionCommand.ToSessionModel(loggedInUser.Id, command);
            await _repository.QrSession.AddAsync(session);
            await _repository.SaveAsync();

            return new OkResponse<CreateQrResponseDto>(new CreateQrResponseDto(session.Id));
        }


        public async Task<ApiBaseResponse> GenerateQrToken(string? userId, Guid sessionId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                return new ForbiddenResponse("Access Deniel");
            }

            var loggedInUser = await _userManager.FindByIdAsync(userId);
            if (loggedInUser == null)
            {
                return new ForbiddenResponse("User not found");
            }

            var session = await _repository.QrSession
                .FirstOrDefault(s => s.Id == sessionId && s.IsActive && s.EndsAt > DateTime.UtcNow);
                if(session == null)
            {
                return new NotFoundResponse("session not found");
            }

            var IsStaff = await _userManager.IsInRoleAsync(loggedInUser, Roles.Staff.ToString());
            if (IsStaff && loggedInUser.Id != session.CreatedById)
            {
                return new ForbiddenResponse(" you are not allowed to perform this operation");
            }

            var token = _tokenService.GenerateQrToken(sessionId);

            return new OkResponse<QrTokenDto>(new QrTokenDto(token, session.RegenerateTokenInSeconds));


        }
    }
}
