using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MT_NetCore_API.Models.RequestModels
{
    public class UpdateIdentityDetailsRequestModel
    {
        [Required]
        [JsonProperty("identity_number")]
        public string IdentityNumber { get; set; }

        [Required]
        [JsonProperty("id_string")]
        public string IdString { get; set; }

        [Required]
        [JsonProperty("password")]
        public string Password { get; set; }
    }
}
