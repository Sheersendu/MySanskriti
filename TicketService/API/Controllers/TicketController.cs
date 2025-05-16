using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TicketService.API.DTOs;
using TicketService.Application.Exceptions;
using TicketService.Application.UseCases;
using TicketService.Domain.Entities;
using TicketService.Domain.Exceptions;

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
	public async Task<IActionResult> GetTicket([FromQuery] Guid bookingId)
	{
		if (bookingId == Guid.Empty)
			return BadRequest("Invalid booking ID.");
		try
		{
			Ticket ticket = await _getTicketHandler.Handle(bookingId);
			return Ok(_mapper.Map<Ticket, TicketResponse>(ticket));
		}
		catch (BookingNotFoundException exception)
		{
			return NotFound(exception.Message);
		}
		catch (Exception exception)
		{
			return StatusCode(500, exception.Message);
		}
	}
	
	[HttpPost("create")]
	public async Task<IActionResult> CreateTicket([FromBody] TicketRequest ticketRequest)
	{
		Ticket ticket = await _createTicketHandler.Handle(ticketRequest);
		TicketResponse? response = _mapper.Map<Ticket, TicketResponse>(ticket);
		return Ok(response);
	}
	
	[HttpPost("cancel")]
	public async Task<IActionResult> CancelTicket([FromBody] TicketRequest ticketRequest)
	{
		try
		{
			Ticket ticket = await _cancelTicketHandler.Handle(ticketRequest);
			TicketResponse? response = _mapper.Map<Ticket, TicketResponse>(ticket);
			return Ok(response);
		}
		catch (BookingNotFoundException exception)
		{
			return NotFound(exception.Message);
		}
		catch (TicketAlreadyCancelledException exception)
		{
			return BadRequest(exception.Message);
		}
		catch (Exception exception)
		{
			return StatusCode(500, exception.Message);
		}
		
	}
}