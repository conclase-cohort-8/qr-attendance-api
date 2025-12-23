using QrAttendanceApi.Application.Abstractions;
using QrAttendanceApi.Application.Commands.Departments;
using QrAttendanceApi.Application.DTOs;
using QrAttendanceApi.Application.Responses;
using QrAttendanceApi.Application.Services.Abstractions;
using QrAttendanceApi.Domain.Entities;

namespace QrAttendanceApi.Application.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IRepositoryManager _repository;

        public DepartmentService(IRepositoryManager repository)
        {
            _repository = repository;
        }

        public async Task<ApiBaseResponse> CreateAsync(AddDepartmentCommand command)
        {
            if(string.IsNullOrWhiteSpace(command.Name))
            {
                return new BadRequestResponse("Department name is required.");
            }

            await _repository.Department
                .AddAsync(new Department
                {
                    Name = command.Name,
                    Description = command.Description,
                });

            return new OkResponse<string>("Department added successfully.");
        }

        public async Task<ApiBaseResponse> GetDepartments()
        {
            var data = (await _repository.Department
                .GetAll())
                .Select(DepartmentDto.ToDto).ToList();

            return new OkResponse<List<DepartmentDto>>(data);
        }
    }
}
