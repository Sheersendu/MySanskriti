using PaymentService.Application.Interfaces;
using PaymentService.Domain.Entities;

namespace PaymentService.Application.UseCases;

public class GetPaymentsForUserIdHandler(IPaymentRepository paymentRepository)
{
	public async Task<List<Payment>> Handle(Guid userId)
	{
		return await paymentRepository.GetAllPaymentsForUserIdAsync(userId);
	}
}