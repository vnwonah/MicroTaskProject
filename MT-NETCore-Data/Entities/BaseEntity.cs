using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MT_NetCore_Data.Entities
{
    public class BaseEntity
    {
        [Key]
        public long Id { get; set; }

        public string UpdatedBy { get; set; }

        [JsonProperty("created_at")]
        public DateTime UTCCreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UTCModifiedAt { get; set; }

        public DateTime? UTCDeletedAt { get; set; }

        [DefaultValue(false)]
        public bool IsDeleted { get; set; }
    }
}
