using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MT_NetCore_Data.IdentityDB
{
    public class AuthenticationDbContext : IdentityDbContext<ApplicationUser>
    {
            
        public AuthenticationDbContext(DbContextOptions<AuthenticationDbContext> _options)
            : base(_options)
        {
        }
    }
}
