using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MT_NetCore_Utils.Enums;

namespace MT_NetCore_API.Interfaces
{
    public interface IAuthorizationService
    {
        bool CurrentUserCanAddUsersToTeam { get; }
        bool CurrentUserCanAddUsersToProject { get; }
        bool CurrentUserCanAddUsersToForm { get; }
        Task<Role> CurrentUserRoleInTeam { get; }
        Role CurrentUserRoleInProject { get; }
        Role CurrentUserRoleInForm { get; }

    }
}
