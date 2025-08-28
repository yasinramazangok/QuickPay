using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickPay.Application.DTOs
{
    public record PaymentRequestDto
    (
        string CardNumber,
        string Expiry, // MM/YY or MM/YYYY
        string Cvv,
        decimal Amount,
        string Currency = "USD"
    );
}
