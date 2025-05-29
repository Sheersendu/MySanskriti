namespace PaymentService.Domain.Entities;

public class Payment
{
	public Guid PaymentId { get; set; } = Guid.NewGuid();
	public required Guid BookingId { get; set; }
	public Guid UserId { get; set; } = Guid.NewGuid();
	public string Currency { get; set; } = "INR";
	public required double Amount { get; set; }
	public string Status { get; set; }
	public required string TransactionId { get; set; }
	public required DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}