using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using MT_NetCore_Utils.Enums;

namespace MT_NetCore_Data.Entities
{
    public class User : BaseEntity
    {
        [JsonProperty("first_name")]
        public string FirstName { get; set; }
        [JsonProperty("last_name")]
        public string LastName { get; set; }
        [JsonProperty("bvn_number")]
        public string BVNNumber { get; set; }
        [JsonProperty("gender")]
        public string Gender { get; set; }
        [JsonProperty("primary_location")]
        public Location PrimaryLocation { get; set; }
        [JsonProperty("secondary_location")]
        public Location SecondaryLocation { get; set; }
        [JsonProperty("id_string")]
        public string IdString { get; set; }
        [JsonProperty("photo_string")]
        public string PhotoString { get; set; }
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
