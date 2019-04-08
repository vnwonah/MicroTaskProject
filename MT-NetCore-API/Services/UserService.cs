using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using MT_NetCore_API.Interfaces;
using MT_NetCore_Data.IdentityDB;

namespace MT_NetCore_API.Services
{
    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserService(
            IHttpContextAccessor httpContextAccessor,
            UserManager<ApplicationUser> userManager)
        {
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }

        public async Task<ApplicationUser> GetUserAsync()
        {
            return await _userManager.FindByEmailAsync(
               _httpContextAccessor.HttpContext.
               User.Claims.FirstOrDefault(
                   c => c.Type == 
               "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")
               .Value);

        }
    }
}
