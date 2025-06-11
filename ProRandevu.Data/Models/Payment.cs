using System;

namespace ProRandevu.Data.Models
{
    /// <summary>
    /// Payment record for an appointment or customer.
    /// </summary>
    public class Payment
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public PaymentType Type { get; set; }

        /// <summary>
        /// Optional note about the payment.
        /// </summary>
        public string? Note { get; set; }

        public int CustomerId { get; set; }
        public Customer? Customer { get; set; }
    }

    public enum PaymentType
    {
        Cash,
        Card,
        Debt
    }
}
