using Microsoft.AspNetCore.Mvc;
using TicketService.API.DTOs;

namespace TicketService.API.Controllers;

[Route("api/ticket")]
[ApiController]
public class TicketController : ControllerBase
{
	[HttpGet]
	public TicketResponse GetTicket([FromQuery] Guid ticketId)
	{
		return new TicketResponse();
	}
	
	[HttpPost("create")]
	public TicketResponse CreateTicket([FromBody] CreateTicketRequest createTicketRequest)
	{
		return new TicketResponse();
	}
	
	[HttpPost("cancel")]
	public TicketResponse CancelTicket([FromQuery] Guid ticketId)
	{
		return new TicketResponse();
	}
	
}