using EventService.Domain.Entities;

namespace EventService.Application.Interfaces;

public interface IEventRepository
{
	Task<Event> CreateEvent(Event newEvent);
	Task<Event> UpdateEvent(Event existingEvent);
	Task<Event> GetEventByEventId(Guid eventId);
	Task<List<Event>> GetEventByCityAndOrEventType(string city, string eventType, int pageNumber, int pageSize);
}