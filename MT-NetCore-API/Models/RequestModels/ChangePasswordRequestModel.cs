using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace MT_NetCore_API.Models.RequestModels
{
    public class ChangePasswordRequestModel
    {
        [Required]
        [JsonProperty("password")]
        public string Password { get; set; }
        [Required]
        [JsonProperty("new_Password")]
        public string NewPassword { get; set; }
        [Required]
        [JsonProperty("confirm_new_password")]
        public string ConfirmNewPassword { get; set; }
    }
}
