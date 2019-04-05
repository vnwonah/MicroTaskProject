using System;
using Newtonsoft.Json;

namespace MT_NetCore_API.Models.ResponseModels
{
    public class RegisterResponse
    {
        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("first_name")]
        public string FirstName { get; set; }

        [JsonProperty("last_name")]
        public string LastName { get; set; }
    }

}
