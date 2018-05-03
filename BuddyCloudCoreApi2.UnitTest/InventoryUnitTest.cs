using BuddyCloudCoreApi2.DAL.Repository;
using BuddyCloudCoreApi2.DAL.DataContext;
using BuddyCloudCoreApi2.Helper;
using BuddyCloudCoreApi2.Core.Models;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace BuddyCloudCoreApi2.UnitTest
{
    [TestClass]
    public class InventoryUnitTest
    {
        GenericRepository<Stock> _repo;
        DatabaseContext _dbContext;

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<DatabaseContext>();
            options.UseSqlServer(DbConnectionStringHelper.CONNECTIONSTRING);

            _dbContext = new DatabaseContext(options.Options);

            _repo = new GenericRepository<Stock>(_dbContext);
        }

        [TestMethod]
        public async Task Inventory_AddStockAsync_Test()
        {
            var stockId = Guid.NewGuid();
            var sku = Guid.NewGuid();

            var stock = new Stock
            {
                StockId = stockId,
                StockName = "Nike shoes for men",
                SKU = sku,
                Description = "Nike shoes for men",
                Qty = 5,
                DateAdded = DateTime.UtcNow,
                UnitPrice = 1450,
                MarkupPrice = 500,
                SellingPrice = 1950,
                UserId = Guid.Parse("6BFED575-0954-4421-B911-DA908D8B1553")
            };

            var isStockAdded = await _repo.AddAsync(stock);

            Assert.IsTrue(isStockAdded);
        }

        [TestMethod]
        public async Task Inventory_GetStocksPerSellerAsync_Test()
        {
            var sellerId = Guid.Parse("56C83DB9-4BCB-4938-A06E-C331FD9F23B0");

            var stocks = await _repo.GetStocksPerSellerAsync(sellerId);

            Assert.IsNotNull(stocks);
        }

        [TestMethod]
        public async Task Inventory_GetStockInfoAsync_Test()
        {
            var sellerId = Guid.Parse("6FED48D1-2EF5-4DCF-91FA-297C9B8EC0CF");
            var stockId = Guid.Parse("ADBF1339-7CED-4FC1-AC6A-417F566D3EA6");

            var stockInfo = await _repo.GetAsync(s => s.UserId == sellerId && s.StockId == stockId);

            Assert.IsNotNull(stockInfo);
        }

        [TestMethod]
        public async Task Inventory_ReplenishStockQtyAsync_Test()
        {
            var sellerId = Guid.Parse("6FED48D1-2EF5-4DCF-91FA-297C9B8EC0CF");
            var stockId = Guid.Parse("ADBF1339-7CED-4FC1-AC6A-417F566D3EA6");
            int qty = 20;

            var isStockReplenished = await _repo.ReplenishItemCountAsync(sellerId, stockId, qty);

            Assert.IsTrue(isStockReplenished);
        }

        [TestMethod]
        public async Task Inventory_UpdateStockAsync_Test()
        {
            var stockId = Guid.Parse("ADBF1339-7CED-4FC1-AC6A-417F566D3EA6");
            var sellerId = Guid.Parse("6FED48D1-2EF5-4DCF-91FA-297C9B8EC0CF");

            var stockInfo = await _repo.GetAsync(s => s.UserId == sellerId && s.StockId == stockId);
            stockInfo.StockName = "Dummy test item";
            stockInfo.Description = "Dummy test item";
            stockInfo.Qty = 60;
            stockInfo.UnitPrice = 2000;
            stockInfo.MarkupPrice = 2500;
            stockInfo.SellingPrice = 4500;

            var isStockUpdated = await _repo.UpdateAsync(stockInfo);

            Assert.IsTrue(isStockUpdated);
        }

        [TestMethod]
        public void Inventory_GetTotalQtySold_Test()
        {
            var sellerId = Guid.Parse("6BFED575-0954-4421-B911-DA908D8B1553");
            var stockId = Guid.Parse("579B0FCD-D16D-43A9-B6B4-A402CF6DEED8");

            var totalQtySold = _repo.GetTotalQtySold(sellerId, stockId);

            Assert.AreNotEqual(0, totalQtySold);
        }

        [TestMethod]
        public void Inventory_GetTotalStockSales_Test()
        {
            var sellerId = Guid.Parse("6BFED575-0954-4421-B911-DA908D8B1553");
            var stockId = Guid.Parse("F1C177F9-00DB-4EEC-801C-8A4AE0CB39F3");

            var totalStockSales = _repo.GetTotalStockSales(sellerId, stockId);

            Assert.AreNotEqual(0, totalStockSales);
        }
    }
}