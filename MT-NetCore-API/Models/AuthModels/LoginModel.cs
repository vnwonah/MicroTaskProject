using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace MT_NetCore_API.Models.AuthModels
{
    public class LoginModel
    {
        [Required]
        [JsonProperty("email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [JsonProperty("password")]
        public string Password { get; set; }
    }
}
