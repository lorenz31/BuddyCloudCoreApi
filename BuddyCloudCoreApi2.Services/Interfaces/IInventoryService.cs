using BuddyCloudCoreApi2.Core.Models;

using System.Threading.Tasks;
using System.Collections.Generic;
using System;

namespace BuddyCloudCoreApi2.Services.Interfaces
{
    public interface IInventoryService
    {
        Task<bool> AddStockAsync(Stock obj);
        Task<List<Stock>> GetStocksPerSellerAsync(Guid sellerid);
        Task<Stock> GetStockInfo(Guid sellerid, Guid stockid);
        Task<bool> ReplenishStockQtyAsync(Guid sellerid, Guid stockid, int qty);
        Task<bool> UpdateStockAsync(Stock obj);
        Task<List<Stock>> GetAvailableStocksPerSellerAsync(Guid sellerid);
        int GetTotalQtySold(Guid sellerid, Guid stockid);
        decimal GetTotalStockSales(Guid sellerid, Guid stockid);
        Task<bool> SetStockSalePriceAsync(Guid sellerid, Guid stockid, int price, int percent);
    }
}