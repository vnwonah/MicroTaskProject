using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace MT_NetCore_API.Models.RequestModels
{
    public class UpdatePaymentDetailsRequestModel
    {
        [Required]
        [JsonProperty("account_number")]
        public int AccountNumber { get; set; }

        [Required]
        [JsonProperty("bank_name")]
        public string BankName { get; set; }

        [Required]
        [JsonProperty("password")]
        public string Password { get; set; }
    }
}
