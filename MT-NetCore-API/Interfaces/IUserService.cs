using System.Threading.Tasks;
using MT_NetCore_API.Models.AuthModels;
using MT_NetCore_Data.IdentityDB;

namespace MT_NetCore_API.Interfaces
{
    public interface IUserService
    {
        Task<ApplicationUser> GetUserAsync();
    }
}
