using System;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

namespace MT_NetCore_Data.IdentityDB
{
    public class ApplicationUser : IdentityUser
    {
        [JsonProperty("first_name")]
        public string FirstName { get; set; }
        [JsonProperty("last_name")]
        public string LastName { get; set; }
        [JsonProperty("bvn_number")]
        public string BVNNumber { get; set; }
        [JsonProperty("gender")]
        public string Gender { get; set; }
        [JsonProperty("id_string")]
        public string IdString { get; set; }
        [JsonProperty("photo_string")]
        public string PhotoString { get; set; }
        public string Address { get; set; }
    }
}
