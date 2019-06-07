using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MT_NetCore_API.Models.RequestModels
{
    public class ChangeRecordStatusModel
    {
        [Required]
        public long RecordId { get; set; }
        [Required]
        public string Message { get; set; }
    }
}
