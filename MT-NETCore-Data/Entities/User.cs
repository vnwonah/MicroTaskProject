using System;
namespace MT_NetCore_Data.Entities
{
    public class User : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string BVNNumber { get; set; }
        public string Sex { get; set; }
        public Location PrimaryLocation { get; set; }
        public Location SecondaryLocation { get; set; }
        public byte[] IdString { get; set; }
        public byte[] PhotoString { get; set; }

        public string ApplicationUserId { get; set; }
    }


}
