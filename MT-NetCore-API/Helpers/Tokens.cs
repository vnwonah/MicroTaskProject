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
            var response = new
            {
                id = identity.Claims.Single(c => c.Type == "id").Value,
                auth_token = await jwtFactory.GenerateEncodedToken(userName, identity),
                expires_in = (int)jwtOptions.ValidFor.TotalSeconds
            };

            return JsonConvert.SerializeObject(response, serializerSettings);
        }
    }
}

