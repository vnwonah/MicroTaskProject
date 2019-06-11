using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MT_NetCore_API.Models.RequestModels
{
    public class UpdateFormModel
    {

        [Required]
        [JsonProperty("form_json")]
        public JObject FormJson { get; set; }

        [Required]
        [JsonProperty("form_id")]
        public long FormId { get; set; }
    }
}
