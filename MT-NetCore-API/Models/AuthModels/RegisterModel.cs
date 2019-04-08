using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace MT_NetCore_API.Models.AuthModels
{
    public class RegisterModel
    {

        [Required]
#if DEBUG
        [DefaultValue("dev@mobileforms.co")]
#endif
        [JsonProperty("email")]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
#if DEBUG
        [DefaultValue("Dev@12345")]
#endif
        [JsonProperty("password")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$", 
            ErrorMessage = "Password must be at least 8 characters " +
        	"and must contain at least 1 uppercase letter, 1 lowercase letter, 1 numerical value and 1 special character")]
        public string Password { get; set; }

        [Required]
#if DEBUG
        [DefaultValue("Dev@12345")]
#endif
        [JsonProperty("confirm_password")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
            ErrorMessage = "Password must be at least 8 characters " +
            "and must contain at least 1 uppercase letter, 1 lowercase letter, 1 numerical value and 1 special character")]
        public string ConfirmPassword { get; set; }

        [Required]
#if DEBUG
        [DefaultValue("Developer")]
#endif
        [RegularExpression("^[\\w'\\-,.][^0-9_!¡?÷?¿/\\\\+=@#$%ˆ&*(){}|~<>;:[\\]]{2,}$", ErrorMessage = "Please enter a valid name with bwtween 3 - 10 letters")]
        [JsonProperty("first_name")]
        public string FirstName { get; set; }


        [Required]
#if DEBUG
        [DefaultValue("Mobile Forms")]
#endif
        [RegularExpression("^[\\w'\\-,.][^0-9_!¡?÷?¿/\\\\+=@#$%ˆ&*(){}|~<>;:[\\]]{2,}$", ErrorMessage = "Please enter a valid name with bwtween 3 - 10 letters")]
        [JsonProperty("last_name")]
        public string LastName { get; set; }


        [Required]
#if DEBUG
        [DefaultValue("09060697346")]
#endif
        [JsonProperty("phone_number")]
        //[RegularExpression("/^\\s*(?:\\+?(\\d{1,3}))?([-. (]*(\\d{3})[-. )]*)?((\\d{3})[-. ]*(\\d{2,4})(?:[-.x ]*(\\d+))?)\\s*$/gm")]
        public string PhoneNumber { get; set; }
    }
}
