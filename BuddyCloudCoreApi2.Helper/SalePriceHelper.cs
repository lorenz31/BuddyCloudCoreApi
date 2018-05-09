using System;
using System.Collections.Generic;
using System.Text;

namespace BuddyCloudCoreApi2.Helper
{
    public class SalePriceHelper
    {
        public static int ComputeSalePrice(int percent, double price)
        {
            var discount = ((float)percent / 100) * price;
            var salePrice = price - discount;

            return (int)salePrice;
        }
    }
}
