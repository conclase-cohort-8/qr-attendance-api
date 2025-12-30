using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using QrAttendanceApi.Application.Abstractions;
using QrAttendanceApi.Application.Abstractions.Externals;
using QrAttendanceApi.Application.Services.Abstractions;
using QrAttendanceApi.Application.Settings;
using QrAttendanceApi.Domain.Entities;

namespace QrAttendanceApi.Application.Services
{
    public class ServiceManager : IServiceManager
    {
        private readonly Lazy<IAccountService> _accountService;
        private readonly Lazy<IDepartmentService> _departmentService;
        private readonly Lazy<IQrSessionService> _qrSessionService;

        public ServiceManager(UserManager<User> userManager, 
                              SignInManager<User> signInManager,
                              IOptions<JwtSettings> options,
                              IRepositoryManager repository,
                              ITokenService tokenService)
        {
            _accountService = new Lazy<IAccountService>(() 
                => new AccountService(userManager, signInManager, options, repository, tokenService));
            _departmentService = new Lazy<IDepartmentService>(()
                => new DepartmentService(repository));
            _qrSessionService = new Lazy<IQrSessionService>(()
                => new QrSessionService(repository));
        }

        public IAccountService Account => _accountService.Value;
        public IDepartmentService Department => _departmentService.Value;
        public IQrSessionService QrSession => _qrSessionService.Value;
    }
}