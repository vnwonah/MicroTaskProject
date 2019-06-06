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
        public int ProjectId { get; set; }
        
        public long NumberOFSubmissions { get; set; }

        public long NumberOFApprovedSubmissions { get; set; }

        public long NumberOFUnApprovedSubmissions { get; set; }
        [JsonProperty("form_users")]
        public ICollection<FormUser> FormUsers { get; set; }

    }

}
