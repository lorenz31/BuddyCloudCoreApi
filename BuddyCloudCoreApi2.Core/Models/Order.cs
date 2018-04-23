using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BuddyCloudCoreApi2.Core.Models
{
    public class Order : BaseEntity
    {
        [Required]
        public string ItemName { get; set; }

        [Required]
        public int Total { get; set; }

        [Required]
        public DateTime DateOrdered { get; set; }

        [ForeignKey("TransactionId")]
        public Transactions Transaction { get; set; }

        [Required]
        public int TransactionId { get; set; }
    }
}