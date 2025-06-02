using PaymentService.Application.Interfaces;
using PaymentService.Domain.Entities;
using PaymentService.Domain.Enums;

namespace PaymentService.Application.UseCases;

public class CreatePaymentHandler(IPaymentRepository paymentRepository)
{
	public async Task<Payment> Handle(double amount, Guid bookingId, string transactionId, DateTime createdAt, PaymentStatus paymentStatus)
	{
		Payment payment = new()
		{
			Amount = amount,
			BookingId = bookingId,
			TransactionId = transactionId,
			CreatedAt = createdAt,
			Status = paymentStatus.ToString(),
		};
		
		return await paymentRepository.CreatePaymentAsync(payment);
	}
}