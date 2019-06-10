using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using MT_NetCore_Utils.Enums;
using Newtonsoft.Json;

namespace MT_NetCore_API.Models.RequestModels
{
    public class AddUserRequest
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
