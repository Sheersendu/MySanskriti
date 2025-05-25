using EventService.API.DTOs;
using EventService.Application.DTOs;
using EventService.Application.Interfaces;
using EventService.Domain.Entities;
using EventService.Domain.Enums;

namespace EventService.Application.UseCases;

public class CreateEventHandler(IEventRepository eventRepository, ILocationClient locationClient)
{
	public async Task<Event> Handle(EventRequest eventRequest)
	{
		Event newEvent = await CreateEventFromEventRequest(eventRequest);
		return await eventRepository.CreateEvent(newEvent);
	}

	private async Task<Event> CreateEventFromEventRequest(EventRequest eventRequest)
	{
		var formattedEventTiming = eventRequest.Timing.ToString(Constants.DateFormat);
		LocationDTO locationDetails = await locationClient.GetLocationByLocationId(eventRequest.LocationId);
		var address = String.Concat(locationDetails.BuildingName, "\n", locationDetails.Street, ", ", locationDetails.City, "\n", locationDetails.State, "\n", locationDetails.PostalCode);
		return new Event
		{
			Name = eventRequest.Name,
			Description = eventRequest.Description,
			Type = eventRequest.Type,
			Timing = formattedEventTiming,
			LocationId = eventRequest.LocationId,
			Status = EventStatus.ACTIVE.ToString(),
			Address = address,
			City = locationDetails.City
		};
	}
}