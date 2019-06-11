using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MT_NetCore_API.Models.AuthModels
{
    public class ApplicationLoginModel
    {
        [Required]
        [JsonProperty("app_name")]
        public string AppName { get; set; }
        [Required]
        [JsonProperty("app_key")]
        public string AppKey { get; set; }
    }
}
