using LocationService.Application.Interfaces;
using LocationService.Domain.Entities;

namespace LocationService.Application.UseCases;

public class GetLocationHandler(ILocationRepository locationRepository, ILogger<GetLocationHandler> logger)
{
	private readonly ILogger<GetLocationHandler> _logger = logger;

	public async Task<IEnumerable<Location>> GetAllLocationsByCity(string city)
	{
		return await locationRepository.GetAllLocationsByCity(city);
	}
}