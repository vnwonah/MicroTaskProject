using Newtonsoft.Json;
using System;
namespace MT_NetCore_Data.Entities
{
    public class Location : BaseEntity
    {

        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
