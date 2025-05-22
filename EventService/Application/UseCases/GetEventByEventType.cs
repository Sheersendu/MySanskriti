using EventService.Application.Interfaces;
using EventService.Domain.Entities;

namespace EventService.Application.UseCases;

public class GetEventByEventType(IEventRepository eventRepository)
{
	public Task<List<Event>> Handle(string eventType)
	{
		return eventRepository.GetEventByEventType(eventType);
	}
}