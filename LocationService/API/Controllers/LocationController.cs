using LocationService.API.DTOs;
using LocationService.Application.UseCases;
using LocationService.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace LocationService.API.Controllers;

[Route("api/location")]
[ApiController]
public class LocationController : ControllerBase
{
	private readonly GetLocationHandler _getLocationHandler;
	private readonly AddLocationHandler _addLocationHandler;
	private readonly UpdateLocationHandler _updateLocationHandler;
	
	public LocationController(GetLocationHandler getLocationHandler, AddLocationHandler addLocationHandler, UpdateLocationHandler updateLocationHandler)
	{
		_getLocationHandler = getLocationHandler;
		_addLocationHandler = addLocationHandler;
		_updateLocationHandler = updateLocationHandler;
	}
	
	[HttpGet]
	public async Task<IActionResult> GetLocationByCity([FromQuery] string city)
	{
		if (string.IsNullOrEmpty(city))
			return BadRequest("City name cannot be empty.");
		
		var locations = await _getLocationHandler.Handle(city);
		return Ok(locations);
	}
	
	[HttpPost("add")]
	public async Task<ActionResult> AddLocation([FromBody] LocationRequest locationRequest)
	{
		Location location = await _addLocationHandler.Handle(locationRequest);
		return Ok(location);
	}
	
	[HttpPut("update/{locationId}")]
	public async Task<ActionResult> UpdateLocation(Guid locationId, [FromBody] LocationRequest locationRequest)
	{
		var updatedLocation = await _updateLocationHandler.Handle(locationId, locationRequest);
		return Ok(updatedLocation);
	}
}