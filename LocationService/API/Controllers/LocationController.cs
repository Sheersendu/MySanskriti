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
	public LocationController(GetLocationHandler getLocationHandler)
	{
		_getLocationHandler = getLocationHandler;
	}
	
	[HttpGet]
	public async Task<IActionResult> GetLocationByCity([FromQuery] string city)
	{
		if (string.IsNullOrEmpty(city))
			return BadRequest("City name cannot be empty.");
		
		var locations = await _getLocationHandler.GetAllLocationsByCity(city);
		return Ok(locations);
	}
	
	[HttpPost("add")]
	public async Task<ActionResult> AddLocation([FromBody] LocationRequest locationRequest)
	{
		
		Location location = new () { Street = locationRequest.street, City = locationRequest.city, State = locationRequest.state, PostalCode = locationRequest.postalCode };

		return Ok(location);
	}
	
	[HttpPut("update")]
	public async Task<ActionResult> UpdateLocation([FromBody] LocationRequest locationRequest)
	{
		
		Location location = new () { Street = locationRequest.street, City = locationRequest.city, State = locationRequest.state, PostalCode = locationRequest.postalCode };

		return Ok(location);
	}
}