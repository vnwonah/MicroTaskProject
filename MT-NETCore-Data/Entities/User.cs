using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using MT_NetCore_Utils.Enums;

namespace MT_NetCore_Data.Entities
{
    public class User : BaseEntity
    {
        [JsonProperty("primary_location")]
        public Location PrimaryLocation { get; set; }
        [JsonProperty("secondary_location")]
        public Location SecondaryLocation { get; set; }
      
        [JsonProperty("team_id")]
        public int TeamId { get; set; }
        [JsonProperty("project_users")]
        public ICollection<ProjectUser> ProjectUsers { get; set; }
        [JsonProperty("form_users")]
        public ICollection<FormUser> FormUsers { get; set; }
        [JsonProperty("email")]
        public string Email { get; set; }
        [JsonProperty("application_user_id")]
        public string ApplicationUserId { get; set; }
        public Role UserRole { get; set; }
    }


}
