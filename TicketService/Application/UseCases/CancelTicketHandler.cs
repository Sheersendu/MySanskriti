using TicketService.API.DTOs;
using TicketService.Application.Interfaces;
using TicketService.Domain.Entities;

namespace TicketService.Application.UseCases;

public class CancelTicketHandler(ITicketRepository ticketRepository)
{
	private ITicketRepository _ticketRepository = ticketRepository;

	public async Task<Ticket> Handle(TicketRequest ticketRequest)
	{
		Ticket existingTicket = await _ticketRepository.GetTicketByBookingId(ticketRequest.bookingId);
		existingTicket.Cancel();
		return await _ticketRepository.CancelTicket(existingTicket);
	}
	
}