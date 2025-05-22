using EventService.API.DTOs;
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
	GetEventByEventType getEventByEventType) : ControllerBase
{
	[HttpGet("{eventId:guid}")]
	public async Task<ActionResult> GetEventByEventId(Guid eventId)
	{
		Event existingEvent = await getEventByEventIdHandler.Handle(eventId);
		return Ok(existingEvent);
	}
	
	[HttpGet("{eventType}")]
	public async Task<ActionResult> GetEventByEventType(string eventType)
	{
		List<Event> events = await getEventByEventType.Handle(eventType);
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
		Event updatedEvent = await updateEventHandler.Handle(eventId, eventRequest);
		return Ok(updatedEvent);
	}
}