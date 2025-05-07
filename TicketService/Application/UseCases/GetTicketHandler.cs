using TicketService.API.DTOs;
using TicketService.Application.Interfaces;
using TicketService.Domain.Entities;

namespace TicketService.Application.UseCases;

public class GetTicketHandler(ITicketRepository ticketRepository)
{
	private ITicketRepository _ticketRepository = ticketRepository;

	public async Task<Ticket> Handle(Guid bookingId)
	{
		Ticket ticket = await _ticketRepository.GetTicketByBookingId(bookingId);
		return ticket;
	}
}