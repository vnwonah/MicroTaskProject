using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace MT_NetCore_API.Models.RequestModels
{
    public class UpdateProfileRequestModel
    {
        [Required]
        [JsonProperty("photo_string")]
        public string PhotoString { get; set; }

        [Required]
        [JsonProperty("gender")]
        public string Gender { get; set; }

        [Required]
        [JsonProperty("address")]
        public string Address { get; set; }

        [Required]
        [JsonProperty("state")]
        public string State { get; set; }

        [Required]
        [JsonProperty("country")]
        public string Country { get; set; }

        [Required]
        [JsonProperty("password")]
        public string Password { get; set; }
    }
}
