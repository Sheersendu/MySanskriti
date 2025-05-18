using LocationService.Application.Interfaces;
using LocationService.Domain.Entities;

namespace LocationService.Application.UseCases;

public class GetLocationHandler(ILocationRepository locationRepository, ILogger<GetLocationHandler> logger)
{
	public async Task<IEnumerable<Location>> Handle(string city)
	{
		logger.Log(LogLevel.Information,$"All locations for city: {city}");
		return await locationRepository.GetAllLocationsByCity(city);
	}
}