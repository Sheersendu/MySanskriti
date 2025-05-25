using AutoMapper;
using EventService.API.DTOs;
using EventService.Application.Exceptions;
using EventService.Application.UseCases;
using EventService.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace EventService.API;

[ApiController]
[Route("api/event")]
public class EventServiceController(
	CreateEventHandler createEventHandler, 
	UpdateEventHandler updateEventHandler, 
	GetEventByEventIdHandler getEventByEventIdHandler,
	GetEventByEventTypeOrCity getEventByEventTypeOrCity,
	IMapper _mapper) : ControllerBase
{
	[HttpGet("{eventId:guid}")]
	public async Task<ActionResult> GetEventByEventId(Guid eventId)
	{
		try
		{
			Event existingEvent = await getEventByEventIdHandler.Handle(eventId);
			return Ok(existingEvent);
		}
		catch (EventNotFoundException e)
		{
			return NotFound(e.Message);
		}
		catch (Exception e)
		{
			return StatusCode(500, e.Message);
		}
	}
	
	[HttpGet]
	public async Task<ActionResult> GetEventByCityOrEventType([FromQuery] string city, [FromQuery] string? type, [FromQuery] int? pageNumber, [FromQuery] int? pageSize)
	{
		if (string.IsNullOrEmpty(city))
		{
			return BadRequest("Please select a city");
		}
		
		List<Event> events = await getEventByEventTypeOrCity.Handle(city, type, pageNumber, pageSize);
		List<EventResponse> eventResponse = events.Select(e => _mapper.Map<Event, EventResponse>(e)).ToList();
		return Ok(eventResponse);
	}
	
	[HttpPost("create")]
	public async Task<ActionResult> CreateEvent([FromBody] EventRequest eventRequest)
	{
		Event newEvent = await createEventHandler.Handle(eventRequest);
		return Ok(newEvent);
	}

	[HttpPut("{eventId:guid}")]
	public async Task<ActionResult> UpdateEvent(Guid eventId, [FromBody] EventRequest eventRequest)
	{
		try
		{
			Event updatedEvent = await updateEventHandler.Handle(eventId, eventRequest);
			return Ok(updatedEvent);
		}
		catch (EventNotFoundException e)
		{
			return NotFound(e.Message);
		}
		catch (Exception e)
		{
			return StatusCode(500, e.Message);
		}
	}
}