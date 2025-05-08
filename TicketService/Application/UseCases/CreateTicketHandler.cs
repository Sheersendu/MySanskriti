using TicketService.API.DTOs;
using TicketService.Application.Interfaces;
using TicketService.Domain.Entities;

namespace TicketService.Application.UseCases;

public class CreateTicketHandler(ITicketRepository ticketRepository)
{
	private ITicketRepository _ticketRepository = ticketRepository;

	public async Task<Ticket> Handle(TicketRequest ticketRequest)
	{
		Ticket newTicket = new()
		{
			BookingId = ticketRequest.bookingId
		};
		
		Ticket savedTicket = await _ticketRepository.CreateTicket(newTicket);
		return savedTicket;
	}
}