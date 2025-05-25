using System.Text.Json;
using EventService.Application.Interfaces;
using EventService.Domain.Entities;
using Microsoft.Extensions.Caching.Distributed;

namespace EventService.Application.UseCases;

public class GetEventByEventTypeOrCity(IEventRepository eventRepository, IDistributedCache cache, ILogger<GetEventByEventTypeOrCity> logger)
{
	public async Task<List<Event>> Handle(string city, string? eventType, int? pageNumber, int? pageSize)
	{
		pageNumber ??= Constants.PageNumber;

		pageSize ??= Constants.PageSize;

		if (pageNumber == Constants.PageNumber && pageSize == Constants.PageSize && string.IsNullOrEmpty(eventType))
		{
			var cachedEvents = await cache.GetStringAsync(Constants.redisCacheKey);
		
			if (!String.IsNullOrEmpty(cachedEvents) && cachedEvents.Any())
			{
				logger.LogInformation("Returning events from cache for city: {City}, pageNumber: {PageNumber}, pageSize: {PageSize}", city, pageNumber, pageSize);
				return JsonSerializer.Deserialize<List<Event>>(cachedEvents);
			}

			logger.LogInformation("Adding events to cache for city: {City}, pageNumber: {PageNumber}, pageSize: {PageSize}. Fetching from database.", city, pageNumber, pageSize);
			var eventList = await eventRepository.GetEventByCityAndOrEventType(city, eventType, (int)pageNumber, (int)pageSize);
			DistributedCacheEntryOptions cacheEntryOptions = new DistributedCacheEntryOptions()
				.SetAbsoluteExpiration(TimeSpan.FromMinutes(10));
				
			var serializedEvents = JsonSerializer.Serialize(eventList);
			await cache.SetStringAsync(Constants.redisCacheKey, serializedEvents, cacheEntryOptions);
			return eventList;
		}
		
		return await eventRepository.GetEventByCityAndOrEventType(city, eventType, (int)pageNumber, (int)pageSize);

	}
}