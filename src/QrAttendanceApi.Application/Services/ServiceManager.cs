using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using QrAttendanceApi.Application.Abstractions;
using QrAttendanceApi.Application.Services.Abstractions;
using QrAttendanceApi.Application.Settings;
using QrAttendanceApi.Domain.Entities;

namespace QrAttendanceApi.Application.Services
{
    public class ServiceManager : IServiceManager
    {
        private readonly Lazy<IAccountService> _accountService;
        private readonly Lazy<IDepartmentService> _departmentService;

        public ServiceManager(UserManager<User> userManager, 
                              SignInManager<User> signInManager,
                              IOptions<JwtSettings> options,
                              IRepositoryManager repository)
        {
            _accountService = new Lazy<IAccountService>(() => new AccountService(userManager, signInManager, options, repository));
            _departmentService = new Lazy<IDepartmentService>(()
                => new DepartmentService(repository));
        }

        public IAccountService Account => _accountService.Value;
        public IDepartmentService Department => _departmentService.Value;
    }
}