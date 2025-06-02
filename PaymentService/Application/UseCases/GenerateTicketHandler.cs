using PaymentService.API.DTOs;
using PaymentService.Application.Interfaces;

namespace PaymentService.Application.UseCases;

public class GenerateTicketHandler(ITicketClient ticketClient)
{
	public async Task<TicketDTO> Handle(Guid bookingId)
	{
		if (bookingId == Guid.Empty)
		{
			throw new ArgumentException("Booking ID cannot be empty", nameof(bookingId));
		}

		TicketDTO ticket = await ticketClient.GenerateTicket(bookingId);
		if (ticket == null)
		{
			throw new InvalidOperationException("Failed to generate ticket");
		}

		return ticket;
	}
}