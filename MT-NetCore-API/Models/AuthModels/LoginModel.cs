using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace MT_NetCore_API.Models.AuthModels
{
    public class LoginModel
    {
        [Required]
#if DEBUG
        [DefaultValue("dev@mobileforms.co")]
#endif
        [JsonProperty("email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [JsonProperty("password")]
#if DEBUG
        [DefaultValue("Dev@12345")]
#endif
        public string Password { get; set; }

    }
}
