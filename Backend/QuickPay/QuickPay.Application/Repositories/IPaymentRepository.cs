using QuickPay.Domain.Entities;

namespace QuickPay.Application.Repositories
{
    public interface IPaymentRepository
    {
        Task AddAsync(Payment payment);
        Task<Payment?> GetAsync(Guid id);
    }
}
