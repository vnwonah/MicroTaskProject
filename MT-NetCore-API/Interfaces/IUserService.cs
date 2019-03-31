using MT_NetCore_API.Models.AuthModels;
using MT_NetCore_Data.IdentityDB;

namespace MT_NetCore_API.Interfaces
{
    public interface IUserService
    {
        bool Login(LoginModel model);
        ApplicationUser Register(RegisterModel model);
    }
}
