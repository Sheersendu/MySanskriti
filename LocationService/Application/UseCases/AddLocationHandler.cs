using AutoMapper;
using LocationService.API.DTOs;
using LocationService.Application.Interfaces;
using LocationService.Domain.Entities;

namespace LocationService.Application.UseCases;

public class AddLocationHandler(ILocationRepository locationRepository, IMapper mapper, ILogger<GetLocationHandler> logger)
{
	public async Task<Location> Handle(LocationRequest locationRequest)
	{
		Location newLocation = mapper.Map<LocationRequest, Location>(locationRequest);
		logger.Log(LogLevel.Information, $"Adding location with street: {locationRequest.street}, city: {locationRequest.city}, state: {locationRequest.state}, postalcode: {locationRequest.postalCode}");
		return await locationRepository.AddLocation(newLocation);
	}
}