using Newtonsoft.Json;

namespace MT_NetCore_API.Models.ResponseModels
{
    public class ErrorResponse : BaseResponse
    {
        [JsonProperty("data")]
        public ErrorData Data { get; set; }
    }

    public class ErrorData
    {
        [JsonProperty("error_description")]
        public string ErrorDescription { get; set; } = "An Error Occured";
    }
}
