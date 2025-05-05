using TicketService.Domain.Enums;

namespace TicketService.Domain.Entities;

public class Ticket
{
	public Guid TicketId { get; set; } = Guid.NewGuid();
	public Guid BookingId { get; set; }
	public DateTime CreatedTimestamp { get; set; } = DateTime.Now;
	public TicketStatus Status { get; set; } = TicketStatus.CONFIRMED;

	public Ticket() { }

	public Ticket(Guid bookingId)
	{
		BookingId = bookingId;
	}
	
	public void Cancel()
	{
		Status = TicketStatus.CANCELLED;
	}
}
