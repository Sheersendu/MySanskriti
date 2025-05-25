using EventService.Application.Interfaces;
using EventService.Domain.Entities;

namespace EventService.Application.UseCases;

public class GetEventByEventTypeOrCity(IEventRepository eventRepository)
{
	public Task<List<Event>> Handle(string city, string? eventType, int? pageNumber, int? pageSize)
	{
		pageNumber ??= Constants.PageNumber;

		pageSize ??= Constants.PageSize;
		return eventRepository.GetEventByCityAndOrEventType(city, eventType, (int)pageNumber, (int)pageSize);
	}
}