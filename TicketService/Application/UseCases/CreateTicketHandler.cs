using TicketService.API.DTOs;
using TicketService.Application.Interfaces;
using TicketService.Domain.Entities;

namespace TicketService.Application.UseCases;

public class CreateTicketHandler(ITicketRepository ticketRepository)
{
	private ITicketRepository _ticketRepository = ticketRepository;

	public async Task<TicketResponse> Handle(TicketRequest ticketRequest)
	{
		Ticket newTicket = new()
		{
			BookingId = ticketRequest.bookingId
		};
		
		Ticket savedTicket = await _ticketRepository.CreateTicket(newTicket);
		return MapToTicketResponse(savedTicket);
	}
	
	private TicketResponse MapToTicketResponse(Ticket ticket)
	{
		TicketResponse ticketResponse = new TicketResponse
		{
			ticketId = ticket.TicketId,
			bookingId = ticket.BookingId,
			ticketStatus = ticket.Status.ToString()
		};
		return ticketResponse;
	}
}