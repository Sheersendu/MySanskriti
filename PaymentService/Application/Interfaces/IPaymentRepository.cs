using PaymentService.Domain.Entities;

namespace PaymentService.Application.Interfaces;

public interface IPaymentRepository
{
	Task<List<Payment>> GetAllPaymentsForUserIdAsync(Guid userId);
	
	Task<Payment> CreatePaymentAsync(Payment payment);
}