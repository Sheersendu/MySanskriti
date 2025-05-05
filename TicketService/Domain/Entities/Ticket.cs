using TicketService.Domain.Enums;

namespace TicketService.Domain.Entities;

public class Ticket(Guid bookingId)
{
	private Guid ticketID = new();
	private Guid bookingID = bookingId;
	private DateTime createdTimestamp = DateTime.Now;
	private TicketStatus status = TicketStatus.CONFIRMED;

	public void Cancel()
	{
		status = TicketStatus.CANCELLED;
	}
}
