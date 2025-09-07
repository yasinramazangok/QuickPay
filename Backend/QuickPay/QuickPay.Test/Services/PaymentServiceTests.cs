using FluentAssertions;
using Moq;
using QuickPay.Application.DTOs;
using QuickPay.Application.Repositories;
using QuickPay.Application.Services;
using QuickPay.Domain.Entities;
using Xunit;

namespace QuickPay.Test.Services
{
    public class PaymentServiceTests
    {
        private readonly Mock<IPaymentRepository> _mockRepo;

        public PaymentServiceTests()
        {
            _mockRepo = new Mock<IPaymentRepository>();
        }

        [Fact]
        public async Task ProcessPayment_InvalidCard_ReturnsFailed()
        {
            // Arrange
            var service = new PaymentService(_mockRepo.Object, new FixedRandom(0.5));
            var request = new PaymentRequestDto
            {
                CardNumber = "1111", // invalid
                Expiry = "12/25",
                Cvv = "123",
                Amount = 100,
                Currency = "USD"
            };

            // Act
            var result = await service.ProcessPaymentAsync(request);

            // Assert
            result.Success.Should().BeFalse();
            result.Message.Should().Contain("Kart numarası");
            _mockRepo.Verify(r => r.AddAsync(It.IsAny<Payment>()), Times.Never);
        }

        [Fact]
        public async Task ProcessPayment_InvalidExpiry_ReturnsFailed()
        {
            var service = new PaymentService(_mockRepo.Object, new FixedRandom(0.5));
            var request = new PaymentRequestDto
            {
                CardNumber = "4111111111111111",
                Expiry = "13/25", // invalid
                Cvv = "123",
                Amount = 100,
                Currency = "USD"
            };

            var result = await service.ProcessPaymentAsync(request);

            result.Success.Should().BeFalse();
            result.Message.Should().Contain("son kullanma tarihi");
            _mockRepo.Verify(r => r.AddAsync(It.IsAny<Payment>()), Times.Never);
        }

        [Fact]
        public async Task ProcessPayment_InvalidCvv_ReturnsFailed()
        {
            var service = new PaymentService(_mockRepo.Object, new FixedRandom(0.5));
            var request = new PaymentRequestDto
            {
                CardNumber = "4111111111111111",
                Expiry = "12/25",
                Cvv = "12", // invalid
                Amount = 100,
                Currency = "USD"
            };

            var result = await service.ProcessPaymentAsync(request);

            result.Success.Should().BeFalse();
            result.Message.Should().Contain("CVV");
            _mockRepo.Verify(r => r.AddAsync(It.IsAny<Payment>()), Times.Never);
        }

        [Fact]
        public async Task ProcessPayment_AmountAboveLimit_ReturnsDeclined()
        {
            var service = new PaymentService(_mockRepo.Object, new FixedRandom(0.5));
            var request = new PaymentRequestDto
            {
                CardNumber = "4111111111111111",
                Expiry = "12/25",
                Cvv = "123",
                Amount = 1500, // above limit
                Currency = "USD"
            };

            var result = await service.ProcessPaymentAsync(request);

            result.Success.Should().BeFalse();
            result.Message.Should().Contain("Ödeme limiti aşıldı");
            _mockRepo.Verify(r => r.AddAsync(It.IsAny<Payment>()), Times.Once);
        }

        [Fact]
        public async Task ProcessPayment_ValidPayment_ReturnsSuccess()
        {
            var service = new PaymentService(_mockRepo.Object, new FixedRandom(0.5)); // decline < 0.1
            var request = new PaymentRequestDto
            {
                CardNumber = "4111111111111111",
                Expiry = "12/25",
                Cvv = "123",
                Amount = 500,
                Currency = "USD"
            };

            _mockRepo.Setup(r => r.AddAsync(It.IsAny<Payment>())).ReturnsAsync(1);

            var result = await service.ProcessPaymentAsync(request);

            result.Success.Should().BeTrue();
            result.Message.Should().Contain("411111******1111"); // masked
            _mockRepo.Verify(r => r.AddAsync(It.Is<Payment>(p => p.Amount == 500)), Times.Once);
        }

        [Fact]
        public async Task ProcessPayment_RandomDecline_ReturnsDeclined()
        {
            var service = new PaymentService(_mockRepo.Object, new FixedRandom(0.05)); // decline < 0.1
            var request = new PaymentRequestDto
            {
                CardNumber = "4111111111111111",
                Expiry = "12/25",
                Cvv = "123",
                Amount = 500,
                Currency = "USD"
            };

            _mockRepo.Setup(r => r.AddAsync(It.IsAny<Payment>())).ReturnsAsync(1);

            var result = await service.ProcessPaymentAsync(request);

            result.Success.Should().BeFalse();
            result.Message.Should().Contain("Bankadan reddedildi");
            _mockRepo.Verify(r => r.AddAsync(It.IsAny<Payment>()), Times.Once);
        }
    }
}
