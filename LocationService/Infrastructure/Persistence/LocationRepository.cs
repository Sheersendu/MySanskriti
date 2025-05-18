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
		var location = await locationDbContext.Location.Where(location => location.LocationId == locationId).ToListAsync();
		Location existingLocation = location.First();
		return existingLocation;
	}
}