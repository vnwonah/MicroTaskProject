using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MT_NetCore_API.Models.RequestModels
{
    public class CreateFormModel
    {
        [Required]
        [JsonProperty("form_name")]
        public string FormName { get; set; }

        [Required]
        [JsonProperty("project_id")]
        public int ProjectId { get; set; }

        [Required]
        [JsonProperty("form_json")]
        public JObject FormJson { get; set; }

        [Required]
        [JsonProperty("form_id")]
        public long FormId { get; set; }

        [Required]
        [JsonProperty("form_title")]
        public string FormTitle { get; set; }
    }
}
