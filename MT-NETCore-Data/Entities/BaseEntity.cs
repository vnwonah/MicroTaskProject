using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MT_NetCore_Data.Entities
{
    public class BaseEntity
    {
        [Key]
        public string Id { get; set; }

        public string UpdatedBy { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTimeOffset CreatedAt { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTimeOffset? ModifiedAt { get; set; }

        public DateTimeOffset? DeletedAt { get; set; }

        [DefaultValue(false)]
        public bool IsDeleted { get; set; }
    }
}
