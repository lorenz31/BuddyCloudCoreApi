﻿using BuddyCloudCoreApi21.DAL.DTO;
using BuddyCloudCoreApi21.DAL.Repository;
using BuddyCloudCoreApi21.Services.Interfaces;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BuddyCloudCoreApi21.Services.Service
{
    public class StatisticService : IStatisticService
    {
        private IEFRepository<StatisticsDTO> _repo;

        public StatisticService(IEFRepository<StatisticsDTO> repo)
        {
            _repo = repo;
        }

        public async Task<StatisticsDTO> GetSalesStatisticsAsync(Guid sellerid, Guid stockid, int month, int year)
        {
            try
            {
                int qty = 0;
                int total = 0;
                string stockName = null;

                var stockId = Guid.Parse(stockid.ToString());

                var statistics = await _repo.GetSalesStatisticsAsync(sellerid, stockid, month, year);

                if (statistics.Count == 0)
                {
                    return null;
                }
                else
                {
                    foreach (var item in statistics)
                    {
                        qty += item.Qty;
                        total += item.TotalSales;
                        stockName = item.StockName;
                    }

                    return new StatisticsDTO
                    {
                        StockName = stockName,
                        Qty = qty,
                        TotalSales = total,
                        Month = month,
                        Year = year
                    };
                }
            }
            catch (ArgumentNullException)
            {
                return null;
            }
            catch (FormatException)
            {
                return null;
            }
        }
    }
}