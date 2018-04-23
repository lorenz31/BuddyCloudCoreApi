using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;

namespace BuddyCloudCoreApi2.Core.Models
{
    public class Transactions : BaseEntity
    {
        [Required]
        public DateTime TransactionDate { get; set; }

        [Required]
        public decimal Subtotal { get; set; }

        [ForeignKey("CustomerId")]
        public Customer Customer { get; set; }

        public int CustomerId { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}