namespace PaymentService.API.DTOs;

public class PaymentRequest
{
	public double amount { get; set; }
	public Guid bookingId { get; set; }
}