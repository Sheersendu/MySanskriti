using TicketService.API.DTOs;
using TicketService.Application.Interfaces;
using TicketService.Domain.Entities;

namespace TicketService.Application.UseCases;

public class CancelTicketHandler(ITicketRepository ticketRepository)
{
	private ITicketRepository _ticketRepository = ticketRepository;

	public async Task<TicketResponse> Handle(TicketRequest ticketRequest)
	{
		Ticket existingTicket = await _ticketRepository.GetTicketByBookingId(ticketRequest.bookingId);
		existingTicket.Cancel();
		await _ticketRepository.CancelTicket(existingTicket);
		return MapToTicketResponse(existingTicket);
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