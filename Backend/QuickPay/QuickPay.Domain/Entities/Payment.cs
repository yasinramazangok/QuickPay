using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickPay.Domain.Entities
{
    public class Payment
    {
        public Guid Id { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; } = "USD";
        public string CardLast4 { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public string? Reason { get; set; }
    }
}
