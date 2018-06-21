using BuddyCloudCoreApi21.Services.Interfaces;
using BuddyCloudCoreApi21.DAL.Repository;
using BuddyCloudCoreApi21.DAL.DTO;
using BuddyCloudCoreApi21.Core.Models;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;


namespace BuddyCloudCoreApi21.Services.Service
{
    public class TransactionService : ITransactionService
    {
        private IEFRepository<Transactions> _repo;

        public TransactionService(IEFRepository<Transactions> repo)
        {
            _repo = repo;
        }

        public async Task<bool> AddTransactionAsync(Transactions obj)
        {
            try
            {
                if (string.IsNullOrEmpty(obj.CustomerName))
                    return false;

                if (obj.Subtotal == 0)
                    return false;

                if (string.IsNullOrEmpty(obj.UserId.ToString()))
                    return false;

                if (obj.Orders == null || obj.Orders.Count == 0)
                    return false;

                if (obj.PaymentType == "Cash on Delivery")
                    obj.BankName = "N/A";

                if (obj.PaymentType == "Bank Deposit")
                    obj.BankName = obj.BankName;

                var transactionDto = new TransactionDTO
                {
                    CustomerName = obj.CustomerName,
                    PaymentType = obj.PaymentType,
                    BankName = obj.BankName,
                    TransactionDate = DateTime.UtcNow,
                    UserId = obj.UserId,
                    Subtotal = obj.Subtotal
                };

                int transactionId = await _repo.AddTransactionAsync(transactionDto);

                if (transactionId > 0)
                {
                    foreach (var order in obj.Orders)
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
                        _repo.DeductItemQty(obj.UserId, order.StockId, order.Qty);
                    }

                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        public async Task<List<Transactions>> GetTransactionHistoryAsync(Guid sellerid)
        {
            var transHist = await _repo.GetTransactionHistoryAsync(sellerid);

            return transHist != null ? transHist : null;
        }

        public async Task<List<Transactions>> GetPendingTransactionsAsync(Guid sellerid)
        {
            var pendingTrans = await _repo.GetPendingTransactionsAsync(sellerid);

            return pendingTrans != null ? pendingTrans : null;
        }

        public async Task<Transactions> GetTransactionDetailsAsync(int transactionid)
        {
            return await _repo.GetTransactionDetailsAsync(transactionid);
        }

        public bool UpdateOrderStatus(Guid sellerId, int transactionId)
        {
            return _repo.UpdateOrderStatus(sellerId, transactionId);
        }
    }
}