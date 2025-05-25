using EventService.Application.DTOs;

namespace EventService.Application.Interfaces;

public interface ILocationClient
{
	Task<LocationDTO> GetLocationByLocationId(Guid locationId);
}