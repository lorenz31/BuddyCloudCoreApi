using BuddyCloudCoreApi2.Core.Models;

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace BuddyCloudCoreApi2.Core.Identity
{
    public class ApplicationUser
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public virtual ICollection<Stock> Stocks { get; set; }
        public virtual ICollection<Customer> Customers { get; set; }
    }
}