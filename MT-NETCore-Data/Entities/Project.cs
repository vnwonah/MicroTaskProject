using System;
using System.Collections;
using System.Collections.Generic;

namespace MT_NetCore_Data.Entities
{
    public class Project : BaseEntity
    {
        //will remove
        public string Name { get; set; }

        public string TeamId { get; set; }

        public ICollection<User> Users { get; set; }

        public ICollection<Form> Forms { get; set; }


    }
}
