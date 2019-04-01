using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using MT_NetCore_API.Interfaces;
using MT_NetCore_API.Models.AuthModels;
using Newtonsoft.Json;

namespace MT_NetCore_API.Helpers
{
    public static class Tokens
    {
        public static async Task<string> GenerateJwt(ClaimsIdentity identity, IJwtFactory jwtFactory, string userName, JwtIssuerOptions jwtOptions, JsonSerializerSettings serializerSettings)
        {
            return await jwtFactory.GenerateEncodedToken(userName, identity);
        }
    }
}

