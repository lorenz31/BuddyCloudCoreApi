using System;
using System.Collections.Generic;
using System.Text;

namespace BuddyCloudCoreApi2.DAL.DTO
{
    public class StatisticsDTO
    {
        public string StockName { get; set; }
        public int Qty { get; set; }
        public int TotalSales { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
    }
}
