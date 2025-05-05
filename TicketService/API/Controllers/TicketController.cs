using Microsoft.AspNetCore.Mvc;
using TicketService.API.DTOs;
using TicketService.Application.UseCases;

namespace TicketService.API.Controllers;

[Route("api/ticket")]
[ApiController]
public class TicketController(GetTicketHandler getTicketHandler) : ControllerBase
{
	[HttpGet]
	public async Task<TicketResponse> GetTicket([FromQuery] Guid bookingId)
	{
		return await getTicketHandler.Handle(bookingId);
	}
	
	[HttpPost("create")]
	public TicketResponse CreateTicket([FromBody] TicketRequest ticketRequest)
	{
		return new TicketResponse();
	}
	
	[HttpPost("cancel")]
	public TicketResponse CancelTicket([FromBody] TicketRequest ticketRequest)
	{
		return new TicketResponse();
	}
	
}