using System.Collections.Generic;

namespace MT_NetCore_Data.Entities
{
    public class Form : BaseEntity
    {
        public string FormJson { get; set; }

        public List<Location> CollectionLocations { get; set; }

        public long SubmissionCount { get; set; }

        public string ProjectId { get; set; }

    }
}
