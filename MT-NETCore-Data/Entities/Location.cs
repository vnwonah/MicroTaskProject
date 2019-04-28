using Newtonsoft.Json;
using System;
namespace MT_NetCore_Data.Entities
{
    public class Location : BaseEntity
    {
        [JsonProperty("address")]
        public string Address { get; set; }
        [JsonProperty("country")]
        public string Country { get; set; }
        [JsonProperty("state")]
        public string State { get; set; }
        [JsonProperty("latitude")]
        public string Latitude { get; set; }
        [JsonProperty("longitude")]
        public string Longitude { get; set; }
    }
}
