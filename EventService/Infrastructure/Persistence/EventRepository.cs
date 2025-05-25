using EventService.Application.Exceptions;
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
		if (existingEvent == null)
		{
			throw new EventNotFoundException($"No event for ID: `{eventId}` was found");
		}
		return existingEvent;
	}
	
	public async Task<List<Event>> GetEventByCityAndOrEventType(string city, string eventType, int pageNumber, int pageSize)
	{
		var query = eventDbContext.Events.AsQueryable();
	    if (string.IsNullOrEmpty(eventType))
		{
			query = query.Where(e => EF.Functions.ILike(e.City, city));
		}
		else
		{
			query = query.Where(e =>
				EF.Functions.ILike(e.City, city) &&
				EF.Functions.ILike(e.Type, eventType));
		}
		
		List<Event> eventList = await query.OrderByDescending(e => e.Timing)
			.Skip((pageNumber - 1) * pageSize) // Skip records for previous pages
			.Take(pageSize)
			.ToListAsync();
		return eventList.OrderBy(e => e.Timing).ToList();
	}
}