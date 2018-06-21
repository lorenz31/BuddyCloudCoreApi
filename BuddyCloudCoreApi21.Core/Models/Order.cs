using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BuddyCloudCoreApi21.Core.Models
{
    public class Order : BaseEntity
    {
        public Guid StockId { get; set; }
        public string ItemName { get; set; }
        public int Qty { get; set; }
        public int Total { get; set; }

        public Transactions Transaction { get; set; }
        public int TransactionId { get; set; }
    }
}