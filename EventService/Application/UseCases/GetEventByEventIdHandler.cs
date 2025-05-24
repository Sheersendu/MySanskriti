using EventService.Application.Interfaces;
using EventService.Domain.Entities;

namespace EventService.Application.UseCases;

public class GetEventByEventIdHandler(IEventRepository eventRepository)
{
	public async Task<Event> Handle(Guid eventId)
	{
		return await eventRepository.GetEventByEventId(eventId);
	}
}