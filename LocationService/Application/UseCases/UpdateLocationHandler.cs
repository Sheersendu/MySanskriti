using LocationService.API.DTOs;
using LocationService.Application.Interfaces;
using LocationService.Domain.Entities;

namespace LocationService.Application.UseCases;

public class UpdateLocationHandler(ILocationRepository locationRepository, ILogger<UpdateLocationHandler> logger)
{
	public async Task<Location> Handle(Guid locationId, LocationRequest locationRequest)
	{
		var existingLocation = await locationRepository.GetLocationById(locationId);
		
		logger.Log(LogLevel.Information, $"\nUpdates: \n1. Street: {existingLocation.Street} to {locationRequest.street}, \n2. City: {existingLocation.City} to {locationRequest.city}, " +
										$"\n3. State: {existingLocation.State} to {locationRequest.state}, \n4. PostalCode: {existingLocation.PostalCode} to {locationRequest.postalCode}, " +
										$"\n5. BuildingName: {existingLocation.BuildingName} to {locationRequest.buildingName}");
		existingLocation.BuildingName = locationRequest.buildingName;
		existingLocation.City = locationRequest.city;
		existingLocation.State = locationRequest.state;
		existingLocation.Street = locationRequest.street;
		existingLocation.PostalCode = locationRequest.postalCode;

		await locationRepository.UpdateLocation(existingLocation);
		
		return existingLocation;
	}
}