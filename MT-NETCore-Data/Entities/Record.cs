using System;
using MT_NetCore_Utils.Enums;
using Newtonsoft.Json;

namespace MT_NetCore_Data.Entities
{
    public class Record : BaseEntity
    {
        [JsonProperty("form_id")]
        public long FormId { get; set; }
        [JsonProperty("record_json")]
        public string RecordJson { get; set; }
        [JsonProperty("location")]
        public virtual Location Location { get; set; }
        [JsonProperty("user_id")]
        public long UserId { get; set; }
        [JsonProperty("status")]
        public RecordStatus Status { get; set; }
        [JsonProperty("message")]
        public string Message { get; set; }
        [JsonIgnore]
        public bool SentToDevice { get; set; }
        [JsonProperty("title")]
        public string LocalTitle { get; set; }

    }
}
