using System;
namespace MT_NetCore_Data.Entities
{
    public class Location : BaseEntity
    {
        public string Address { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
    }
}
