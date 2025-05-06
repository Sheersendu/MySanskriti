using TicketService.API.DTOs;
using TicketService.Domain.Entities;

namespace TicketService.Application.Interfaces;

public interface ITicketRepository
{
	Task<Ticket> GetTicketByBookingId(Guid bookingId);
	Task<Ticket> CreateTicket(Ticket ticket);
	Task<Ticket> CancelTicket(Ticket ticket);
}