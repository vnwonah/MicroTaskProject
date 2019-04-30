using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;

namespace MT_NetCore_Data.Entities
{
    public class Project : BaseEntity
    {
        //will remove
        [JsonProperty("name")]
        public string Name { get; set; }
        
        [JsonProperty("team_id")]
        public int TeamId { get; set; }
        
        [JsonProperty("teams")]
        public ICollection<Form> Forms { get; set; }

        [JsonProperty("project_users")]
        public ICollection<ProjectUser> ProjectUsers { get; set; }


    }
}
