namespace TicketService.API.DTOs;

public class TicketResponse
{
	public Guid ticketId { get; set; }
	public Guid bookingId { get; set; }
	public string ticketStatus { get; set; }
}