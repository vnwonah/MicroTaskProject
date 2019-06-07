using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MT_NetCore_API.Models.RequestModels
{
    public class CreateRecordModel
    {
        [JsonProperty("form_id")]
        public long FormId { get; set; }
        [JsonProperty("response_json")]
        public string ResponseJson { get; set; }
        [JsonProperty("latitude")]
        public double Latitude { get; set; }
        [JsonProperty("longitude")]
        public double Longitude { get; set; }
    }
}
