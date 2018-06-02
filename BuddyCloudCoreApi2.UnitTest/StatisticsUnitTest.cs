using BuddyCloudCoreApi2.DAL.DataContext;
using BuddyCloudCoreApi2.DAL.DTO;
using BuddyCloudCoreApi2.DAL.Repository;
using BuddyCloudCoreApi2.Helper;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BuddyCloudCoreApi2.UnitTest
{
    [TestClass]
    public class StatisticsUnitTest
    {
        GenericRepository<StatisticsDTO> _repo;
        DatabaseContext _dbContext;

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<DatabaseContext>();
            options.UseSqlServer(DbConnectionStringHelper.CONNECTIONSTRING);

            _dbContext = new DatabaseContext(options.Options);

            _repo = new GenericRepository<StatisticsDTO>(_dbContext);
        }

        [TestMethod]
        public async Task Statistics_GetSalesStatisticsAsync_Test()
        {
            Guid stockId = Guid.Parse("9A069B39-0A3F-4FCD-97CD-942CFB1C02B5");
            Guid sellerId = Guid.Parse("2802ADD9-E682-40A9-C665-08D5C364BE70");
            int month = 6;
            int year = 2018;

            var sales = await _repo.GetSalesStatisticsAsync(sellerId, stockId, month, year);

            Assert.AreNotEqual(0, sales.Count);
        }
    }
}
