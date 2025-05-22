using LocationService.API.DTOs;
using LocationService.Application.Exceptions;
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
	private readonly GetLocationByLocationIdHandler _getLocationByLocationIdHandler;
	
	public LocationController(GetLocationHandler getLocationHandler, AddLocationHandler addLocationHandler, UpdateLocationHandler updateLocationHandler, GetLocationByLocationIdHandler getLocationByLocationIDHandler)
	{
		_getLocationHandler = getLocationHandler;
		_addLocationHandler = addLocationHandler;
		_updateLocationHandler = updateLocationHandler;
		_getLocationByLocationIdHandler = getLocationByLocationIDHandler;
	}
	
	[HttpGet("{locationId}")]
	public async Task<ActionResult> GetLocationByLocationId(Guid locationId)
	{
		if (locationId == Guid.Empty || locationId == null)
			return BadRequest("Location ID cannot be empty.");
		try
		{
			var location = await _getLocationByLocationIdHandler.Handle(locationId);
			return Ok(location);
		}
		catch (LocationNotFoundException exception)
		{
			return NotFound(exception.Message);
		}
		catch (Exception exception)
		{
			return StatusCode(500, exception.Message);
		}
	}

	[HttpGet]
	public async Task<ActionResult> GetLocationByCity([FromQuery] string city)
	{
		if (string.IsNullOrEmpty(city))
			return BadRequest("City name cannot be empty.");
		try
		{
			var locations = await _getLocationHandler.Handle(city);
			return Ok(locations);
		}
		catch (Exception exception)
		{
			return StatusCode(500, exception.Message);
		}
	}
	
	[HttpPost("add")]
	public async Task<ActionResult> AddLocation([FromBody] LocationRequest locationRequest)
	{
		try
		{
			Location location = await _addLocationHandler.Handle(locationRequest);
			return Ok(location);
		}
		catch (Exception exception)
		{
			return StatusCode(500, exception.Message);
		}
	}
	
	[HttpPut("update/{locationId}")]
	public async Task<ActionResult> UpdateLocation(Guid locationId, [FromBody] LocationRequest locationRequest)
	{
		try
		{
			await _updateLocationHandler.Handle(locationId, locationRequest);
			return NoContent();
		}
		catch (LocationNotFoundException exception)
		{
			return NotFound(exception.Message);
		}
		catch (Exception exception)
		{
			return StatusCode(500, exception.Message);
		}
		
	}
}