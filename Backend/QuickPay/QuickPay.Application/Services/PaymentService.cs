using QuickPay.Application.Common;
using QuickPay.Application.DTOs;
using QuickPay.Application.Repositories;
using QuickPay.Application.Utils;
using QuickPay.Domain.Entities;
using QuickPay.Domain.Enums;

namespace QuickPay.Application.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _repository;
        private readonly IRandom _random;

        public PaymentService(IPaymentRepository repository, IRandom random)
        {
            _repository = repository;
            _random = random;
        }

        public async Task<PaymentResponseDto> ProcessPaymentAsync(PaymentRequestDto request)
        {
            // Normalization
            var normalizedCardNumber = CardValidator.NormalizeCardNumber(request.CardNumber);

            // Validate Luhn
            if (!CardValidator.IsValidLuhn(normalizedCardNumber))
            {
                return new PaymentResponseDto(false, string.Empty, "Kart numarası doğrulanamadı!");
            }

            // Validate expiry
            if (!CardValidator.IsValidExpiry(request.Expiry))
            {
                return new PaymentResponseDto(false, string.Empty, "Kartınızın son kullanma tarihi geçersiz!");
            }

            // Validate CVV (3 or 4 digits)
            if (!CardValidator.IsValidCvv(request.Cvv))
            {
                return new PaymentResponseDto(false, string.Empty, "CVV doğrulanamadı!");
            }

            bool approved = true;
            string reason = "Başarılı!";

            // 1️⃣ High amount check (>1000 USD)
            if (request.Amount > 1000)
            {
                approved = false;
                reason = "Ödeme limiti aşıldı ( simülasyon )";
            }
            // 2️⃣ Random risk decline (%5)
            else if (_random.NextDouble() < 0.10)
            {
                approved = false;
                reason = "Bankadan reddedildi (simülasyon risk)";
            }

            // Masked card number for response
            var maskedCardNumber = normalizedCardNumber.Length < 8 ? string.Empty :
                   normalizedCardNumber.Substring(0, 6) +
                   new string('*', normalizedCardNumber.Length - 10) +
                   normalizedCardNumber.Substring(normalizedCardNumber.Length - 4);

            var transaction = new Payment
            {
                Id = Guid.NewGuid(),
                Amount = request.Amount,
                Currency = request.Currency,
                CardLast4 = normalizedCardNumber.Length >= 4 ? normalizedCardNumber[^4..] : normalizedCardNumber,
                CreatedAt = DateTime.UtcNow,
                Status = approved ? PaymentStatus.Approved : PaymentStatus.Declined,
                Reason = reason,
            };

            await _repository.AddAsync(transaction);

            return new PaymentResponseDto(approved, transaction.Id.ToString(), $"{maskedCardNumber} numaralı kartınızla yapılan işlem sonucu: {transaction.Reason}");
        }
    }

}
