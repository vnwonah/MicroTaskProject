using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace MT_NetCore_API.Models.AuthModels
{
    public class LoginModel
    {
        [Required]
        [JsonProperty("email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [JsonProperty("password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
