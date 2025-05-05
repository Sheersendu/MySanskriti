using Microsoft.EntityFrameworkCore;
using TicketService.Application.Interfaces;
using TicketService.Domain.Entities;

namespace TicketService.Infrastructure.Persistence;

public class TicketRepository(TicketDBContext context) : ITicketRepository
{
	public async Task<Ticket> GetTicketByBookingId(Guid bookingId)
	{
		var tickets = await context.Ticket.Where(ticket => ticket.BookingId == bookingId).ToListAsync();
		var ticket = tickets.FirstOrDefault();
		if (ticket == null)
		{
			throw new Exception($"No ticket for {bookingId} was found");
		}
		return ticket;
	}
}