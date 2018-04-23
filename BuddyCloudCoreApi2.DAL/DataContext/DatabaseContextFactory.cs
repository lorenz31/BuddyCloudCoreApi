using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuddyCloudCoreApi2.DAL.DataContext
{
    public class DatabaseContextFactory : IDesignTimeDbContextFactory<DatabaseContext>
    {
        public DatabaseContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<DatabaseContext>();
            builder.UseSqlServer("Server=LANZ-PC\\SQL2K14;Database=BuddyCloudCoreApiDB;User Id=sa;Password=demo123;Trusted_Connection=True;MultipleActiveResultSets=True");
            //builder.UseSqlServer("Server=mssql6.gear.host;Database=buddyclouddb;User Id=buddyclouddb;Password=Mp3!?jL562uC;Trusted_Connection=True;MultipleActiveResultSets=True");

            return new DatabaseContext(builder.Options);
        }
    }
}