using System.Threading.Tasks;
using MT_NetCore_API.Models.AuthModels;
using MT_NetCore_Data.Entities;
using MT_NetCore_Data.IdentityDB;

namespace MT_NetCore_API.Interfaces
{
    public interface IUserService
    {
        Task<ApplicationUser> GetApplicationUserAsync();

        Task<User> GetCurrentUserAsync(int tenantId);
    }
}
