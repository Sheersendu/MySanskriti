using EventService.API.DTOs;
using EventService.Application.Interfaces;
using EventService.Domain.Entities;

namespace EventService.Application.UseCases;

public class UpdateEventHandler(IEventRepository eventRepository)
{
	public async Task<Event> Handle(Guid eventId, EventRequest eventRequest)
	{
		Event existingEvent = await eventRepository.GetEventByEventId(eventId);
		string formattedEventTiming = eventRequest.Timing.ToString("dd-MM-yyyy HH:mm");
		existingEvent.UpdateDetails(eventRequest.Name, eventRequest.Description, eventRequest.Type, formattedEventTiming, eventRequest.LocationId);
		return await eventRepository.UpdateEvent(existingEvent);
	}
}