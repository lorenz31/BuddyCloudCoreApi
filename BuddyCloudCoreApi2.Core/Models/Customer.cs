using BuddyCloudCoreApi2.Core.Identity;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BuddyCloudCoreApi2.Core.Models
{
    public class Customer : BaseEntity
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string ContactNumber { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }

        public Guid UserId { get; set; }

        public virtual ICollection<Transactions> Transactions { get; set; }
    }
}