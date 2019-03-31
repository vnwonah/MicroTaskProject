using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace MT_NetCore_API.Models.AuthModels
{
    public class RegisterModel
    {
        [Required]
        [JsonProperty("email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [JsonProperty("password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [JsonProperty("confirm_password")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }

        [Required]
        [RegularExpression("^[a-z]{3,10}$", ErrorMessage = "Please enter a valid name with 1 - 10 letters")]
        public string FirstName { get; set; }

        [Required]
        [RegularExpression("^[a-z]{3,10}$", ErrorMessage = "Please enter a valid name with 1 - 10 letters")]
        public string LastName { get; set; }

        [Required]
        public string PhoneNumber { get; set; }
    }
}
