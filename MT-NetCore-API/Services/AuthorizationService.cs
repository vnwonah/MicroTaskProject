using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MT_NetCore_API.Interfaces;
using MT_NetCore_Utils.Enums;

namespace MT_NetCore_API.Services
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly IUserService _userService;
        private readonly int _tenantId;
        public AuthorizationService(
            IUserService userService,
            IRequestContext requestContext)
        {
            _userService = userService;
            _tenantId = requestContext.TenantId;
        }
        public bool CurrentUserCanAddUsersToTeam { get; }
        public bool CurrentUserCanAddUsersToProject { get; }
        public bool CurrentUserCanAddUsersToForm { get; }

        public Task<Role> CurrentUserRoleInTeam
        {
            get { return Task.Run(async () => await GetCurrentUserRoleInTeam()); }
        }

        public Role CurrentUserRoleInProject { get; }
        public Role CurrentUserRoleInForm { get; }

        private async Task<Role> GetCurrentUserRoleInTeam()
        {
            var user = await _userService.GetCurrentUserAsync(_tenantId);
            return user.UserRole;
        }
    }
}
