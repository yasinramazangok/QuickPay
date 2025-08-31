using QuickPay.Domain.Entities;

namespace QuickPay.Application.Repositories
{
    public interface IPaymentRepository
    {
        Task<int> AddAsync(Payment payment);
        Task<Payment?> GetByIdAsync(Guid id);
    }
}
