using Microsoft.EntityFrameworkCore;
using TicketService.API.DTOs;
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
			throw new Exception($"No ticket for Booking ID: `{bookingId}` was found");
		}
		return ticket;
	}
	
	public async Task<Ticket> CreateTicket(Ticket ticket)
	{
		await context.Ticket.AddAsync(ticket);
		await context.SaveChangesAsync();
		return ticket;
	}
}