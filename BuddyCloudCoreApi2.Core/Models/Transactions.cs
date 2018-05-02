using BuddyCloudCoreApi2.Core.Identity;

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;

namespace BuddyCloudCoreApi2.Core.Models
{
    public class Transactions : BaseEntity
    {
        [Required]
        public string CustomerName { get; set; }

        [Required]
        public DateTime TransactionDate { get; set; }

        [Required]
        public decimal Subtotal { get; set; }

        [Required]
        public string PaymentType { get; set; }

        [Required]
        public string BankName { get; set; }

        [Required]
        public bool Status { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }

        public Guid UserId { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}