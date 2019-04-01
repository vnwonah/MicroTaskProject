using System;

namespace MT_NetCore_Data.Entities
{
    public class Submission : BaseEntity
    {
        public byte[] FormId { get; set; }

        public string SubmissionJson { get; set; }

        public long SubmissionPosition { get; set; }
    }
}
