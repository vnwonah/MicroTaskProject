using Newtonsoft.Json;

namespace MT_NetCore_API.Models.ResponseModels
{
    public class ErrorResponse
    {
        [JsonProperty("error_description")]
        public string ErrorDescription { get; set; } = "An Error Occured";
    }
}
