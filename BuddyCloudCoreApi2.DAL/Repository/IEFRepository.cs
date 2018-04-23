using BuddyCloudCoreApi2.Core.Identity;
using BuddyCloudCoreApi2.DAL.DataContext;
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
        Task<bool> AddAsync(TEntity obj);
        Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> condition);
        Task<bool> UpdateAsync(TEntity obj);

        Task<ApplicationUser> GetUserInfoAsync(ApplicationUser model);
        Task<List<Stock>> GetStocksPerSellerAsync(Guid id);
        Task<bool> ReplenishItemCountAsync(Guid sellerid, Guid stockid, int qty);
        Task<List<Stock>> GetAvailableStocksPerSellerAsync(Guid sellerid);
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

        #region Non-generic Methods
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
    }
}