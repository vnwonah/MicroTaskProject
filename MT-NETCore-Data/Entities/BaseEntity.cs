using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MTNetCoreData.Entities
{
    public class BaseEntity
    {
        public byte[] Id { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTimeOffset CreatedAt { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTimeOffset? ModifiedAt { get; set; }

        public DateTimeOffset? DeletedAt { get; set; }
    }
}
