using System.Collections.Generic;

namespace MT_NetCore_Data.Entities
{
    public class Form : BaseEntity
    {
        public string Name { get; set; }

        public string Title { get; set; }

        public string FormJson { get; set; }

        public List<Location> CollectionLocations { get; set; }
        
        public int ProjectId { get; set; }
        
        public List<User> Supervisors { get; set; }

        public long NumberOFSubmissions { get; set; }

        public long NumberOFApprovedSubmissions { get; set; }

        public long NumberOFUnApprovedSubmissions { get; set; }

    }

}
