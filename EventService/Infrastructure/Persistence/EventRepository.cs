using EventService.Application.Interfaces;
using EventService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventService.Infrastructure.Persistence;

public class EventRepository(EventDBContext eventDbContext) : IEventRepository
{
	public async Task<Event> CreateEvent(Event newEvent)
	{
		await eventDbContext.Events.AddAsync(newEvent);
		await eventDbContext.SaveChangesAsync();
		return newEvent;
	}
	
	public async Task<Event> UpdateEvent(Event existingEvent)
	{
		eventDbContext.Entry(existingEvent).State = EntityState.Modified;
		await eventDbContext.SaveChangesAsync();
		return existingEvent;
	}

	public async Task<Event> GetEventByEventId(Guid eventId)
	{
		var existingEvent = await eventDbContext.Events.FirstOrDefaultAsync(e => e.EventId == eventId);
		// if (existingEvent == null)
		// {
		// 	throw new EventNotFoundException($"No event for ID: `{eventId}` was found");
		// }
		return existingEvent;
	}
	
	public Task<List<Event>> GetEventByEventType(string eventType)
	{
		var existingEvent = eventDbContext.Events.Where(e => e.Type == eventType.ToUpper()).ToListAsync();
		return existingEvent;
	}
}