using LocationService.Domain.Entities;

namespace LocationService.Application.Interfaces;

public interface ILocationRepository
{
	Task<IEnumerable<Location>> GetAllLocationsByCity(string city);
	Task<Location> AddLocation(Location location);
	Task<Location> UpdateLocation(Location location);
	Task<Location> GetLocationByLocationId(Guid locationId);
}