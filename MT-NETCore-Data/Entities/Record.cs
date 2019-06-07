using System;
using MT_NetCore_Utils.Enums;

namespace MT_NetCore_Data.Entities
{
    public class Record : BaseEntity
    {
        public long FormId { get; set; }
        public string RecordJson { get; set; }
        public Location Location { get; set; }
        public long UserId { get; set; }
        public RecordStatus Status { get; set; }
        public string Message { get; set; }

    }
}
