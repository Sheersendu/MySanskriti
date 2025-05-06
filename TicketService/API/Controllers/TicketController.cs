using Microsoft.AspNetCore.Mvc;
using TicketService.API.DTOs;
using TicketService.Application.UseCases;

namespace TicketService.API.Controllers;

[Route("api/ticket")]
[ApiController]
public class TicketController : ControllerBase
{
	private readonly GetTicketHandler _getTicketHandler;
	private readonly CreateTicketHandler _createTicketHandler;
	
	public TicketController(GetTicketHandler getTicketHandler, CreateTicketHandler createTicketHandler)
	{
		_getTicketHandler = getTicketHandler;
		_createTicketHandler = createTicketHandler;
	}
	
	[HttpGet]
	public async Task<TicketResponse> GetTicket([FromQuery] Guid bookingId)
	{
		return await _getTicketHandler.Handle(bookingId);
	}
	
	[HttpPost("create")]
	public async Task<TicketResponse> CreateTicket([FromBody] TicketRequest ticketRequest)
	{
		return await _createTicketHandler.Handle(ticketRequest);
	}
	
	[HttpPost("cancel")]
	public TicketResponse CancelTicket([FromBody] TicketRequest ticketRequest)
	{
		return new TicketResponse();
	}
	
}