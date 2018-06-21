using BuddyCloudCoreApi21.Core.Identity;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BuddyCloudCoreApi21.Core.Models
{
    public class Customer : BaseEntity
    {
        public string Name { get; set; }
        public string ContactNumber { get; set; }
        
        public ApplicationUser User { get; set; }

        public Guid UserId { get; set; }

        public virtual ICollection<Transactions> Transactions { get; set; }
    }
}