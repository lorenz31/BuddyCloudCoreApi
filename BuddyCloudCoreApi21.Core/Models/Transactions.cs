using BuddyCloudCoreApi21.Core.Identity;

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;

namespace BuddyCloudCoreApi21.Core.Models
{
    public class Transactions : BaseEntity
    {
        public string CustomerName { get; set; }
        public DateTime TransactionDate { get; set; }
        public decimal Subtotal { get; set; }
        public string PaymentType { get; set; }
        public string BankName { get; set; }
        public bool Status { get; set; }

        public ApplicationUser User { get; set; }
        public Guid UserId { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}