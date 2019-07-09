using Newtonsoft.Json;

namespace MT_NetCore_API.Models.ResponseModels
{
    public class LoginResponse
    {
        [JsonProperty("token")]
        public string Token { get; set; }
        [JsonProperty("user_name")]
        public string UserName { get; set; }
        [JsonProperty("email")]
        public string Email { get; set; }
        [JsonProperty("photo_string")]
        public string PhotoString { get; set; }
    }

}
