using System;
using System.Collections.Generic;
using System.Text;
using MT_NetCore_Utils.Enums;

namespace MT_NetCore_Data.Entities
{
    public class FormUser
    {
        public int UserId { get; set; }
        public User User { get; set; }
        public int FormId { get; set; }
        public Form Form { get; set; }
        public Role UserRole { get; set; }

    }
}
