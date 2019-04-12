using System;
using System.Collections;
using System.Collections.Generic;

namespace MT_NetCore_Data.Entities
{
    public class Project : BaseEntity
    {
        //will remove
        public string Name { get; set; }

        public int TeamId { get; set; }
        
        public ICollection<Form> Forms { get; set; }

        public ICollection<ProjectUser> ProjectUsers { get; set; }


    }
}
