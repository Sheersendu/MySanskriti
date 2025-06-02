namespace PaymentService.API.DTOs;

public class TicketDTO
{
	public Guid ticketId { get; set; }
	public Guid bookingId { get; set; }
	public string ticketStatus { get; set; }
}