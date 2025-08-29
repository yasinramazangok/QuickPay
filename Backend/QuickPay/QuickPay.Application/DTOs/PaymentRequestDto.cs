namespace QuickPay.Application.DTOs
{
    public record PaymentRequestDto
    {
        public string CardNumber { get; init; }
        public string Expiry { get; init; }
        public string Cvv { get; init; }
        public decimal Amount { get; init; }
        public string Currency { get; init; }
    }
}
