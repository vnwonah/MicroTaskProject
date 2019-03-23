using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MTNETStdData.Entities
{
    public class BaseEntity
    {
        public string Id { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTimeOffset CreatedAt { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTimeOffset? ModifiedAt { get; set; }

        public DateTimeOffset? DeletedAt { get; set; }
    }
}
