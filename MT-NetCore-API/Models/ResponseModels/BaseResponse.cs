using MT_NetCore_API.Enums;
using Newtonsoft.Json;

namespace MT_NetCore_API.Models.ResponseModels
{
    public abstract class BaseResponse
    {
        [JsonProperty("status")]
        public ResponseStatus Status { get; set; } = ResponseStatus.Error;

        [JsonProperty("message")]
        public string Message { get; set; } = "An Error Occured";


    }
}
