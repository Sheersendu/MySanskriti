using LocationService.API.DTOs;
using LocationService.Application.Interfaces;
using LocationService.Domain.Entities;

namespace LocationService.Application.UseCases;

public class UpdateLocationHandler(ILocationRepository locationRepository, ILogger<UpdateLocationHandler> logger)
{
	public async Task<Location> Handle(Guid locationId, LocationRequest locationRequest)
	{
		var existingLocation = await locationRepository.GetLocationByLocationId(locationId);
	
		LogChanges(existingLocation, locationRequest);
		existingLocation.UpdateDetails(
			locationRequest.buildingName,
			locationRequest.street,
			locationRequest.city,
			locationRequest.state,
			locationRequest.postalCode
		);
		await locationRepository.UpdateLocation(existingLocation);
	
		return existingLocation;
	}
	
	private void LogChanges(Location oldLocation, LocationRequest updatedLocation)
	{
		logger.LogInformation(
			"Updating Location : {LocationId}. Changes: {@Changes} \n",
			oldLocation.LocationId,
			new
			{
				Street = new { From = oldLocation.Street, To = updatedLocation.street },
				City = new { From = oldLocation.City, To = updatedLocation.city },
				State = new { From = oldLocation.State, To = updatedLocation.state },
				PostalCode = new { From = oldLocation.PostalCode, To = updatedLocation.postalCode },
				BuildingName = new { From = oldLocation.BuildingName, To = updatedLocation.buildingName }
			}
		);
	}
}