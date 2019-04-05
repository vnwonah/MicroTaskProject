using System;
using System.Net;
using MT_NetCore_API.Enums;
using Newtonsoft.Json;

namespace MT_NetCore_API.Models.ResponseModels
{
    public class BaseResponse
    {
        public static BaseResponse Create(HttpStatusCode statusCode, object result = null, string errorMessage = null)
        {
            return new BaseResponse(statusCode, result, errorMessage);
        }

        [JsonProperty("version")]
        public string Version => "2.0.0";

        [JsonProperty("status_code")]
        public int StatusCode { get; set; }

        [JsonProperty("request_id")]
        public string RequestId { get; }

        [JsonProperty("error_message")]
        public string ErrorMessage { get; set; }

        [JsonProperty("data")]
        public object Data { get; set; }

        protected BaseResponse(HttpStatusCode statusCode, object result = null, string errorMessage = null)
        {
            RequestId = Guid.NewGuid().ToString();
            StatusCode = (int)statusCode;
            Data = result;
            ErrorMessage = errorMessage;
        }


    }
}
