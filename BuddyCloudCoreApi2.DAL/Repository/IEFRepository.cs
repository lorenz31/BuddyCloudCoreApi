using BuddyCloudCoreApi2.Core.Identity;
using BuddyCloudCoreApi2.DAL.DataContext;
using BuddyCloudCoreApi2.DAL.DTO;
using BuddyCloudCoreApi2.Core.Models;

using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace BuddyCloudCoreApi2.DAL.Repository
{
    public interface IEFRepository<TEntity> where TEntity : class
    {
        Task<TEntity> GetUserInfoAsync(Expression<Func<TEntity, bool>> condition);

        #region Generic Methods
        Task<bool> AddAsync(TEntity obj);
        Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> condition);
        Task<bool> UpdateAsync(TEntity obj);
        #endregion

        #region Stocks Method
        Task<ApplicationUser> GetUserInfoAsync(ApplicationUser model);
        Task<List<Stock>> GetStocksPerSellerAsync(Guid id);
        Task<bool> ReplenishItemCountAsync(Guid sellerid, Guid stockid, int qty);
        Task<List<Stock>> GetAvailableStocksPerSellerAsync(Guid sellerid);
        #endregion

        #region Orders & Transactions Methods
        Task<int> AddTransactionAsync(TransactionDTO obj);
        void AddOrders(Order obj);
        void DeductItemQty(Guid sellerid, Guid stockid, int qty);
        Task<List<Transactions>> GetTransactionHistoryAsync(Guid sellerid);
        Task<List<Transactions>> GetPendingTransactionsAsync(Guid sellerid);
        Task<Transactions> GetTransactionDetailsAsync(int transactionid);
        bool UpdateOrderStatus(Guid sellerid, int transactionid);
        #endregion

        #region Sales Methods
        int GetTotalQtySold(Guid sellerid, Guid stockid);
        decimal GetTotalStockSales(Guid sellerid, Guid stockid);
        #endregion
    }

    public class GenericRepository<T> : IEFRepository<T> where T : class
    {
        private DatabaseContext _dbContext;
        private DbSet<T> _dbSet;

        public GenericRepository(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<T>();
        }

        public async Task<T> GetUserInfoAsync(Expression<Func<T, bool>> condition)
        {
            var userInfo = await _dbSet.Where(condition).SingleOrDefaultAsync();

            return userInfo;
        }

        #region Generic Methods
        public async Task<bool> AddAsync(T obj)
        {
            try
            {
                _dbContext.Set<T>().Add(obj);
                await _dbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>> condition)
        {
            var entity = await _dbSet.Where(condition).SingleOrDefaultAsync();

            return entity;
        }

        public async Task<bool> UpdateAsync(T obj)
        {
            _dbContext.Entry(obj).State = EntityState.Modified;
            var result = await _dbContext.SaveChangesAsync();

            return result == 1 ? true : false;
        }
        #endregion

        #region Stocks Methods
        public async Task<ApplicationUser> GetUserInfoAsync(ApplicationUser model)
        {
            var userInfo = await _dbContext.Users.Where(u => u.Email == model.Email).SingleOrDefaultAsync();

            return userInfo;
        }

        public async Task<List<Stock>> GetStocksPerSellerAsync(Guid id)
        {
            var stocks = await _dbContext.Stocks.Where(u => u.UserId == id).ToListAsync();

            return stocks;
        }

        public async Task<bool> ReplenishItemCountAsync(Guid sellerid, Guid stockid, int qty)
        {
            var sellrId = Guid.Parse(sellerid.ToString());
            var stkId = Guid.Parse(stockid.ToString());

            var stockInfo = await _dbContext.Stocks.Where(s => s.UserId == sellrId && s.StockId == stkId).SingleAsync();
            stockInfo.Qty += qty;

            _dbContext.Entry(stockInfo).State = EntityState.Modified;
            _dbContext.SaveChanges();

            return true;
        }

        public async Task<List<Stock>> GetAvailableStocksPerSellerAsync(Guid sellerid)
        {
            var stocks = await _dbContext.Stocks.Where(s => s.UserId == sellerid && s.Qty > 0).ToListAsync();

            if (stocks != null)
                return stocks;
            else
                return null;
        }
        #endregion

        #region Orders & Transactions Methods
        public async Task<int> AddTransactionAsync(TransactionDTO obj)
        {
            try
            {
                var transaction = new Transactions
                {
                    CustomerName = obj.CustomerName,
                    PaymentType = obj.PaymentType,
                    BankName = obj.BankName,
                    TransactionDate = obj.TransactionDate,
                    UserId = obj.UserId,
                    Subtotal = obj.Subtotal
                };

                _dbContext.Transactions.Add(transaction);
                await _dbContext.SaveChangesAsync();

                return transaction.Id;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public void AddOrders(Order order)
        {
            try
            {
                _dbContext.Orders.Add(order);
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
            }
        }

        public void DeductItemQty(Guid sellerid, Guid stockid, int qty)
        {
            var item = _dbContext.Stocks.Where(s => s.UserId == sellerid && s.StockId == stockid).SingleOrDefault();
            item.Qty -= qty;

            _dbContext.Entry(item).State = EntityState.Modified;
            _dbContext.SaveChanges();
        }

        public async Task<List<Transactions>> GetTransactionHistoryAsync(Guid sellerid)
        {
            var transHist = await _dbContext.Transactions.Include(trans => trans.Orders)
                                                         .Where(trans => trans.UserId == sellerid)
                                                         .ToListAsync();

            return transHist;
        }

        public async Task<List<Transactions>> GetPendingTransactionsAsync(Guid sellerid)
        {
            var pendingTrans = await _dbContext.Transactions.Where(trans => trans.Status == false && trans.UserId == sellerid).ToListAsync();

            return pendingTrans;
        }

        public async Task<Transactions> GetTransactionDetailsAsync(int transactionid)
        {
            var transHist = await _dbContext.Transactions.Include(trans => trans.Orders)
                                                         .Where(trans => trans.Id == transactionid)
                                                         .SingleAsync();

            return transHist;
        }

        public bool UpdateOrderStatus(Guid sellerid, int transactionid)
        {
            var order = _dbContext.Transactions.Where(t => t.UserId == sellerid && t.Id == transactionid).SingleOrDefault();
            order.Status = true;

            _dbContext.Entry(order).State = EntityState.Modified;
            var isUpdated = _dbContext.SaveChanges();

            return isUpdated == 1 ? true : false;
        }
        #endregion

        #region Sales Methods
        public int GetTotalQtySold(Guid sellerid, Guid stockid)
        {
            var stockInfo = _dbContext.Stocks.Where(stock => stock.UserId == sellerid && stock.StockId == stockid).FirstOrDefault();
            var totalQtySold = _dbContext.Orders.Where(order => order.StockId == stockInfo.StockId)
                                                .Select(order => order.Qty)
                                                .Sum();

            return totalQtySold;
        }

        public decimal GetTotalStockSales(Guid sellerid, Guid stockid)
        {
            var stockInfo = _dbContext.Stocks.Where(stock => stock.UserId == sellerid && stock.StockId == stockid).FirstOrDefault();
            var totalStockSales = _dbContext.Orders.Where(order => order.StockId == stockInfo.StockId)
                                                .Select(order => order.Total)
                                                .Sum();

            return totalStockSales;
        }
        #endregion
    }
}