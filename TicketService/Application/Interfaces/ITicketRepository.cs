using TicketService.Domain.Entities;

namespace TicketService.Application.Interfaces;

public interface ITicketRepository
{
	Task<Ticket> GetTicketByBookingId(Guid bookingId);
}