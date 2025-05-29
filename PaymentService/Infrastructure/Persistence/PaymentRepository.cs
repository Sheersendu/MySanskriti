using Microsoft.EntityFrameworkCore;
using PaymentService.Application.Interfaces;
using PaymentService.Domain.Entities;

namespace PaymentService.Infrastructure.Persistence;

public class PaymentRepository(PaymentDBContent dbContent) : IPaymentRepository
{
	public async Task<List<Payment>> GetAllPaymentsForUserIdAsync(Guid userId)
	{
		return await dbContent.Payment.Where(payment => payment.UserId == userId).ToListAsync();
	}
	
	public async Task<Payment> CreatePaymentAsync(Payment payment)
	{
		await dbContent.Payment.AddAsync(payment);
		await dbContent.SaveChangesAsync();
		return payment;
	}
}