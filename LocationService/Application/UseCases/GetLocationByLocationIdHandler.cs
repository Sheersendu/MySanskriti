using LocationService.Application.Interfaces;
using LocationService.Domain.Entities;

namespace LocationService.Application.UseCases;

public class GetLocationByLocationIdHandler(ILocationRepository locationRepository)
{

	public async Task<Location> Handle(Guid locationId)
	{
		return await locationRepository.GetLocationByLocationId(locationId);
	}
}