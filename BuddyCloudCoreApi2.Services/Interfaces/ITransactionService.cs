using BuddyCloudCoreApi2.Core.Models;
using BuddyCloudCoreApi2.DAL.DTO;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BuddyCloudCoreApi2.Services.Interfaces
{
    public interface ITransactionService
    {
        Task<bool> AddTransactionAsync(Transactions obj);
        Task<List<Transactions>> GetTransactionHistoryAsync(Guid sellerid);
        Task<List<Transactions>> GetPendingTransactionsAsync(Guid sellerid);
        Task<Transactions> GetTransactionDetailsAsync(int transactionid);
        bool UpdateOrderStatus(Guid sellerId, int transactionId);
    }
}