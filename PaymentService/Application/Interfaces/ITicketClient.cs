using PaymentService.API.DTOs;

namespace PaymentService.Application.Interfaces;

public interface ITicketClient
{
	Task<TicketDTO> GenerateTicket(Guid bookingId);
}