using TicketService.API.DTOs;
using TicketService.Application.Interfaces;
using TicketService.Domain.Entities;
using TicketService.Domain.Enums;
using TicketService.Domain.Exceptions;

namespace TicketService.Application.UseCases;

public class CancelTicketHandler(ITicketRepository ticketRepository)
{
	private ITicketRepository _ticketRepository = ticketRepository;

	public async Task<Ticket> Handle(TicketRequest ticketRequest)
	{
		try
		{
			Ticket existingTicket = await _ticketRepository.GetTicketByBookingId(ticketRequest.bookingId);
			if (existingTicket.Status == TicketStatus.CANCELLED)
			{
				throw new TicketAlreadyCancelledException("Ticket is already cancelled.");
			}
			existingTicket.Cancel();
			return await _ticketRepository.CancelTicket(existingTicket);
		}
		catch (Exception ex)
		{
            throw ex;
        }
	}
	
}