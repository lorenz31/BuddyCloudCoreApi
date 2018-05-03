using BuddyCloudCoreApi2.DAL.Repository;
using BuddyCloudCoreApi2.DAL.DataContext;
using BuddyCloudCoreApi2.DAL.DTO;
using BuddyCloudCoreApi2.Helper;
using BuddyCloudCoreApi2.Core.Models;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace BuddyCloudCoreApi2.UnitTest
{
    [TestClass]
    public class TransactionUnitTest
    {
        GenericRepository<Transactions> _repo;
        DatabaseContext _dbContext;

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<DatabaseContext>();
            options.UseSqlServer(DbConnectionStringHelper.CONNECTIONSTRING);

            _dbContext = new DatabaseContext(options.Options);

            _repo = new GenericRepository<Transactions>(_dbContext);
        }

        [TestMethod]
        public async Task Transaction_AddOrdersAsync_Test()
        {
            var transaction = new Transactions
            {
                CustomerName = "Customer 1",
                PaymentType = "Bank Deposit",
                BankName = "Bank of the Philippine Islands",
                TransactionDate = DateTime.UtcNow,
                Subtotal = 1500,
                UserId = Guid.Parse("6BFED575-0954-4421-B911-DA908D8B1553"),
                Orders = new List<Order>
                {
                    //new Order
                    //{
                    //    ItemName = "iphone x",
                    //    Qty = 2,
                    //    Total = 107000,
                    //    StockId = Guid.Parse("A12C256E-A071-444E-8770-056EAC476A92")
                    //}
                }
            };

            if (string.IsNullOrEmpty(transaction.CustomerName))
                Assert.Fail();

            if (string.IsNullOrEmpty(transaction.PaymentType))
                Assert.Fail();

            if (string.IsNullOrEmpty(transaction.BankName))
                Assert.Fail();

            if (transaction.Subtotal == 0)
                Assert.Fail();

            if (string.IsNullOrEmpty(transaction.UserId.ToString()))
                Assert.Fail();

            if (transaction.Orders == null || transaction.Orders.Count == 0)
                Assert.Fail();

            var transactionDto = new TransactionDTO
            {
                CustomerName = transaction.CustomerName,
                PaymentType = transaction.PaymentType,
                BankName = transaction.BankName,
                TransactionDate = transaction.TransactionDate,
                UserId = transaction.UserId,
                Subtotal = transaction.Subtotal
            };

            int transactionId = await _repo.AddTransactionAsync(transactionDto);

            Assert.AreNotEqual(0, transactionId);

            if (transactionId > 0)
            {
                foreach (var order in transaction.Orders)
                {
                    var orders = new Order
                    {
                        TransactionId = transactionId,
                        ItemName = order.ItemName,
                        Total = order.Total,
                        Qty = order.Qty,
                        StockId = order.StockId
                    };

                    _repo.AddOrders(orders);
                }

                Assert.IsTrue(true);
            }
            else
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public async Task Transaction_GetTransactionHistory_Test()
        {
            var sellerId = Guid.Parse("6BFED575-0954-4421-B911-DA908D8B1553");

            var transHist = await _repo.GetTransactionHistoryAsync(sellerId);

            Assert.IsNotNull(transHist);
        }
    }
}