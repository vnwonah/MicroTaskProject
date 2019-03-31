using Newtonsoft.Json;

namespace MT_NetCore_API.Models.ResponseModels
{
    public class LoginResponse : BaseResponse
    {
        [JsonProperty("data")]
        public LoginData Data { get; set; }
    }

    public class LoginData
    {
        [JsonProperty("token")]
        public string Token { get; set; }
    }

}
