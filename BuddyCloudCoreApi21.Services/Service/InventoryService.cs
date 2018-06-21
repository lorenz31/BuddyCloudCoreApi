using BuddyCloudCoreApi21.Core.Models;
using BuddyCloudCoreApi21.Services.Interfaces;
using BuddyCloudCoreApi21.DAL.Repository;
using BuddyCloudCoreApi21.Helper;

using System.Threading.Tasks;
using System;
using System.Collections.Generic;

namespace BuddyCloudCoreApi21.Services.Service
{
    public class InventoryService : IInventoryService
    {
        private IEFRepository<Stock> _repo;

        public InventoryService(IEFRepository<Stock> repo)
        {
            _repo = repo;
        }

        public async Task<bool> AddStockAsync(Stock obj)
        {
            if (obj != null)
            {
                if (string.IsNullOrEmpty(obj.StockName))
                    return false;

                if (string.IsNullOrEmpty(obj.Description))
                    return false;

                if (obj.Qty == 0)
                    return false;

                if (obj.UnitPrice == 0)
                    return false;

                if (obj.MarkupPrice == 0)
                    return false;

                if (obj.SellingPrice == 0)
                    return false;

                var stockId = Guid.NewGuid();
                var sku = Guid.NewGuid();

                var stock = new Stock
                {
                    UserId = Guid.Parse(obj.UserId.ToString()),
                    StockId = stockId,
                    StockName = obj.StockName,
                    SKU = sku,
                    Description = obj.Description,
                    Qty = obj.Qty,
                    DateAdded = DateTime.UtcNow,
                    UnitPrice = obj.UnitPrice,
                    MarkupPrice = obj.MarkupPrice,
                    SellingPrice = obj.SellingPrice
                };

                var isStockAdded = await _repo.AddAsync(stock);

                if (isStockAdded)
                    return true;
                else
                    return false;
            }

            return false;
        }

        public async Task<List<Stock>> GetStocksPerSellerAsync(Guid sellerid)
        {
            var stocks = await _repo.GetStocksPerSellerAsync(sellerid);

            if (stocks != null)
                return stocks;
            else
                return null;
        }

        public async Task<Stock> GetStockInfo(Guid sellerid, Guid stockid)
        {
            var stockInfo = await _repo.GetAsync(s => s.UserId == sellerid && s.StockId == stockid);

            if (stockInfo != null)
                return stockInfo;
            else
                return null;
        }

        public async Task<bool> ReplenishStockQtyAsync(Guid sellerid, Guid stockid, int qty)
        {
            var id = Guid.Parse(sellerid.ToString());
            var stkId = Guid.Parse(stockid.ToString());

            var isStockCountReplenished = await _repo.ReplenishItemCountAsync(id, stkId, qty);

            if (isStockCountReplenished)
                return true;
            else
                return false;
        }

        public async Task<bool> UpdateStockAsync(Stock obj)
        {
            var currentStockInfo = await _repo.GetAsync(s => s.UserId == obj.UserId && s.StockId == obj.StockId);
            currentStockInfo.StockName = obj.StockName;
            currentStockInfo.Qty = obj.Qty;
            currentStockInfo.Description = obj.Description;
            currentStockInfo.UnitPrice = obj.UnitPrice;
            currentStockInfo.MarkupPrice = obj.MarkupPrice;
            currentStockInfo.SellingPrice = obj.SellingPrice;

            var isStockUpdated = await _repo.UpdateAsync(currentStockInfo);

            return isStockUpdated == true ? true : false;
        }

        public async Task<List<Stock>> GetAvailableStocksPerSellerAsync(Guid sellerid)
        {
            var stocks = await _repo.GetAvailableStocksPerSellerAsync(sellerid);

            return stocks != null ? stocks : null;
        }

        public int GetTotalQtySold(Guid sellerid, Guid stockid)
        {
            int totalQtySold = _repo.GetTotalQtySold(sellerid, stockid);

            return totalQtySold == 0 ? 0 : totalQtySold;
        }

        public decimal GetTotalStockSales(Guid sellerid, Guid stockid)
        {
            decimal totalStockSales = _repo.GetTotalStockSales(sellerid, stockid);

            return totalStockSales == 0 ? 0 : totalStockSales;
        }

        public async Task<bool> SetStockSalePriceAsync(Guid sellerid, Guid stockid, int price, int percent)
        {
            var salePrice = SalePriceHelper.ComputeSalePrice(percent, price);

            return await _repo.SetStockSalePriceAsync(sellerid, stockid, salePrice);
        }
    }
}