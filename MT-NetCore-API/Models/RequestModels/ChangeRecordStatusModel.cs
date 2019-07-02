using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MT_NetCore_API.Models.RequestModels
{
    public class ChangeRecordStatusModel
    {
        [Required]
        [JsonProperty("record_id")]
        public long RecordId { get; set; }
        [Required]
        [JsonProperty("message")]
        public string Message { get; set; }
    }
}
