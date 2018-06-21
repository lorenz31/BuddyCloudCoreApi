using BuddyCloudCoreApi21.Core.Models;

using System;
using System.Collections.Generic;

namespace BuddyCloudCoreApi21.Core.Identity
{
    public class ApplicationUser
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public string Salt { get; set; }

        public virtual ICollection<Stock> Stocks { get; set; }
    }
}