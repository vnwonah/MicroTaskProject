using MT_NetCore_Utils.Enums;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace MT_NetCore_API.Models.RequestModels
{
    public class AddUserRequestModel
    {
        [Required]
        [JsonProperty("email")]
        public string Email { get; set; }
        [Required]
        [JsonProperty("role")]
        public Role Role { get; set; }
        [JsonProperty("id")]
        public long Id { get; set; }
    }
}
