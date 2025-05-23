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
	GetEventByEventTypeOrCity getEventByEventTypeOrCity) : ControllerBase
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
	public async Task<ActionResult> GetEventByEventType([FromQuery] string city, [FromQuery] string? type)
	{
		if ((city == null || string.IsNullOrEmpty(city)) && (type == null || string.IsNullOrEmpty(type)))
		{
			return BadRequest("Please provide either a city or an event type.");
		}
		
		List<Event> events = await getEventByEventTypeOrCity.Handle(city, type);
		return Ok(events);
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