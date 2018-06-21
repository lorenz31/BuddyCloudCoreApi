using BuddyCloudCoreApi21.DAL.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BuddyCloudCoreApi21.Services.Interfaces
{
    public interface IStatisticService
    {
        Task<StatisticsDTO> GetSalesStatisticsAsync(Guid sellerid, Guid stockid, int month, int year);
    }
}
