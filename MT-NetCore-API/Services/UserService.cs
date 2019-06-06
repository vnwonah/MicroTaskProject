using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using MT_NetCore_API.Interfaces;
using MT_NetCore_Common.Interfaces;
using MT_NetCore_Data.Entities;
using MT_NetCore_Data.IdentityDB;

namespace MT_NetCore_API.Services
{
    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITenantRepository _tenantRepository;
        

        public UserService(
            IHttpContextAccessor httpContextAccessor,
            UserManager<ApplicationUser> userManager,
            ITenantRepository tenantRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _tenantRepository = tenantRepository;
        }

        public async Task<ApplicationUser> GetApplicationUserAsync()
        {
            return await _userManager.FindByEmailAsync(
               _httpContextAccessor.HttpContext.
               User.Claims.FirstOrDefault(
                   c => c.Type == 
               "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")
               ?.Value);

        }

       
        public async Task<User> GetCurrentUserAsync(int tenantId)
        {
            var appUser = await GetApplicationUserAsync();
            return await _tenantRepository.GetUserByEmailAsync(appUser.Email, tenantId);
        }
    }
}
