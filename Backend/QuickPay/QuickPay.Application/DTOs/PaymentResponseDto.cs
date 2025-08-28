using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickPay.Application.DTOs
{
    public record PaymentResponseDto
    (
        bool Success,
        string TransactionId,
        string? Message
    );
}
