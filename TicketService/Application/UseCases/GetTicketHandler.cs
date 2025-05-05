using TicketService.API.DTOs;
using TicketService.Application.Interfaces;
using TicketService.Domain.Entities;

namespace TicketService.Application.UseCases;

public class GetTicketHandler(ITicketRepository ticketRepository)
{
	private ITicketRepository _ticketRepository = ticketRepository;

	public async Task<TicketResponse> Handle(Guid bookingId)
	{
		Ticket ticket = await _ticketRepository.GetTicketByBookingId(bookingId);
		return MapToTicketResponse(ticket);
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