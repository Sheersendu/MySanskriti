using EventService.Application.DTOs;
using EventService.Application.Interfaces;

namespace EventService.Infrastructure.Persistence;

public class LocationClient(HttpClient httpClient) : ILocationClient
{
	public async Task<LocationDTO> GetLocationByLocationId(Guid locationId)
	{
		var endpoint = $"/api/location/{locationId}";
		var response = await httpClient.GetAsync(endpoint);
		response.EnsureSuccessStatusCode();
		return await response.Content.ReadFromJsonAsync<LocationDTO>();
	}
}