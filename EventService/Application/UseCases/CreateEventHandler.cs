using EventService.API.DTOs;
using EventService.Application.Interfaces;
using EventService.Domain.Entities;
using EventService.Domain.Enums;

namespace EventService.Application.UseCases;

public class CreateEventHandler(IEventRepository eventRepository)
{
	public async Task<Event> Handle(EventRequest eventRequest)
	{
		Event newEvent = CreateEventFromEventRequest(eventRequest);
		return await eventRepository.CreateEvent(newEvent);
	}

	private Event CreateEventFromEventRequest(EventRequest eventRequest)
	{
		var dateFormat = "dd-MM-yyyy HH:mm";
		var formattedEventTiming = eventRequest.Timing.ToString(dateFormat);
		return new Event
		{
			Name = eventRequest.Name,
			Description = eventRequest.Description,
			Type = eventRequest.Type,
			Timing = formattedEventTiming,
			LocationId = eventRequest.LocationId,
			Status = EventStatus.ACTIVE.ToString()
		};
	}
}