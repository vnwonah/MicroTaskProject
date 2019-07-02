using Newtonsoft.Json;
using System.Collections.Generic;

namespace MT_NetCore_Data.Entities
{
    public class Form : BaseEntity
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("form_json")]
        public string FormJson { get; set; }

        public List<Location> CollectionLocations { get; set; }

        [JsonProperty("project_id")]
        public long ProjectId { get; set; }
        
        public long SubmissionCount { get; set; }
        public long DeletedCount { get; set; }
        public long ApprovedCount { get; set; }
        public long RejectedCount { get; set; }
        public long InvalidatedCount { get; set; }
        [JsonProperty("form_users")]
        public ICollection<FormUser> FormUsers { get; set; }

        public long MaxNumberOfAgents { get; set; }
        public double PricePerResponse { get; set; }
        public string Description { get; set; }

    }

}
