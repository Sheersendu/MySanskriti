using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TicketService.API.DTOs;
using TicketService.Application.UseCases;
using TicketService.Domain.Entities;

namespace TicketService.API.Controllers;

[Route("api/ticket")]
[ApiController]
public class TicketController : ControllerBase
{
	private readonly GetTicketHandler _getTicketHandler;
	private readonly CreateTicketHandler _createTicketHandler;
	private readonly CancelTicketHandler _cancelTicketHandler;
	private readonly IMapper _mapper;
	
	public TicketController(GetTicketHandler getTicketHandler, CreateTicketHandler createTicketHandler, CancelTicketHandler cancelTicketHandler, IMapper mapper)
	{
		_getTicketHandler = getTicketHandler;
		_createTicketHandler = createTicketHandler;
		_cancelTicketHandler = cancelTicketHandler;
		_mapper = mapper;
	}
	
	[HttpGet]
	public async Task<TicketResponse> GetTicket([FromQuery] Guid bookingId)
	{
		Ticket ticket = await _getTicketHandler.Handle(bookingId);
		return _mapper.Map<Ticket, TicketResponse>(ticket);
	}
	
	[HttpPost("create")]
	public async Task<TicketResponse> CreateTicket([FromBody] TicketRequest ticketRequest)
	{
		Ticket ticket = await _createTicketHandler.Handle(ticketRequest);
		return _mapper.Map<Ticket, TicketResponse>(ticket);
	}
	
	[HttpPost("cancel")]
	public async Task<TicketResponse> CancelTicket([FromBody] TicketRequest ticketRequest)
	{
		Ticket ticket = await _cancelTicketHandler.Handle(ticketRequest);
		return _mapper.Map<Ticket, TicketResponse>(ticket);
	}
}