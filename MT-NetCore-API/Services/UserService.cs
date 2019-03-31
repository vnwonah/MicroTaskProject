using System;
using MT_NetCore_API.Interfaces;
using MT_NetCore_API.Models.AuthModels;
using MT_NetCore_Data.IdentityDB;

namespace MT_NetCore_API.Services
{
    public class UserService : IUserService
    {

        public UserService()
        {
        }

        public bool Login(LoginModel model)
        {
            return true;
        }

        public ApplicationUser Register(RegisterModel model)
        {
            throw new NotImplementedException();
        }
    }
}
