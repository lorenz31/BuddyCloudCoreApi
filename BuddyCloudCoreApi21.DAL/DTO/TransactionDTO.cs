using System;

namespace BuddyCloudCoreApi21.DAL.DTO
{
    public class TransactionDTO
    {
        public string CustomerName { get; set; }
        public string PaymentType { get; set; }
        public string BankName { get; set; }
        public DateTime TransactionDate { get; set; }
        public decimal Subtotal { get; set; }
        public Guid UserId { get; set; }
    }
}