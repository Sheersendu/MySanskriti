using LocationService.Application.Exceptions;
using LocationService.Application.Interfaces;
using LocationService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LocationService.Infrastructure.Persistence;

public class LocationRepository(LocationDBContext locationDbContext) : ILocationRepository
{
	public async Task<IEnumerable<Location>> GetAllLocationsByCity(string city)
	{
		var locations = await locationDbContext.Location.Where(location => location.City.ToLower() == city.ToLower()).ToListAsync();
		return locations;
	}
	
	public async Task<Location> AddLocation(Location location)
	{
		await locationDbContext.Location.AddAsync(location);
		await locationDbContext.SaveChangesAsync();
		return location;
	}
	
	public async Task<Location> UpdateLocation(Location location)
	{
		locationDbContext.Entry(location).State = EntityState.Modified;
		await locationDbContext.SaveChangesAsync();
		return location;
	}
	
	public async Task<Location> GetLocationById(Guid locationId)
	{
		var existingLocation = await locationDbContext.Location.FirstOrDefaultAsync(location => location.LocationId == locationId);
		if (existingLocation == null)
		{
			throw new LocationNotFoundException($"No location for ID: `{locationId}` was found");
		}
		return existingLocation;
	}
}