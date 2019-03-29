using Newtonsoft.Json;

namespace MT_NetCore_API.Models.ResponseModels
{
    public class LoginResponse
    {
        [JsonProperty("token")]
        public string Token { get; set; }
    }

}
