using BuddyCloudCoreApi2.Core.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BuddyCloudCoreApi2.Core.Models
{
    public class Stock
    {
        [Key]
        public Guid StockId { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string StockName { get; set; }

        [Required]
        public Guid SKU { get; set; }

        public string Description { get; set; }

        [Required]
        public int Qty { get; set; }

        [Required]
        public DateTime DateAdded { get; set; }

        [Required]
        public decimal UnitPrice { get; set; }

        [Required]
        public decimal MarkupPrice { get; set; }

        [Required]
        public decimal SellingPrice { get; set; }

        public decimal SalePrice { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }

        [Required]
        public Guid UserId { get; set; }
    }
}