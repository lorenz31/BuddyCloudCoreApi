using BuddyCloudCoreApi21.Core.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BuddyCloudCoreApi21.Core.Models
{
    public class Stock
    {
        public Guid StockId { get; set; }
        public string StockName { get; set; }
        public Guid SKU { get; set; }
        public string Description { get; set; }
        public int Qty { get; set; }
        public DateTime DateAdded { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal MarkupPrice { get; set; }
        public decimal SellingPrice { get; set; }

        public decimal SalePrice { get; set; }
        public virtual ApplicationUser User { get; set; }
        public Guid UserId { get; set; }
    }
}