using System;
using System.Collections.Generic;

namespace MT_NetCore_Data.Entities
{
    public class User : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string BVNNumber { get; set; }
        public string Gender { get; set; }
        public Location PrimaryLocation { get; set; }
        public Location SecondaryLocation { get; set; }
        public string IdString { get; set; }
        public string PhotoString { get; set; }
        public int TeamId { get; set; }
        public ICollection<ProjectUser> ProjectUsers { get; set; }
        public string Email { get; set; }

        public string ApplicationUserId { get; set; }


    }


}
