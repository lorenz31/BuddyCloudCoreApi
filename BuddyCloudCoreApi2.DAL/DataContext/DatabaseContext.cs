using BuddyCloudCoreApi2.Core.Identity;
using BuddyCloudCoreApi2.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace BuddyCloudCoreApi2.DAL.DataContext
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }

        public DbSet<ApplicationUser> Users { get; set; }
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Transactions> Transactions { get; set; }
    }
}