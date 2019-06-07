using System;
using MT_NetCore_Utils.Enums;

namespace MT_NetCore_Data.Entities
{
    //Join Table for many to many relationship between Projects and Users.
    public class ProjectUser 
    {
        public long UserId { get; set; }
        public User User { get; set; }
        public long ProjectId { get; set; }
        public Project Project { get; set; }
        public Role UserRole { get; set; }


    }
}
