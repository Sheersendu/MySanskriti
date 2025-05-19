using AutoMapper;
using LocationService.API.Controllers;
using LocationService.API.DTOs;
using LocationService.Application.Exceptions;
using LocationService.Application.Interfaces;
using LocationService.Application.Mappings;
using LocationService.Application.UseCases;
using LocationService.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Guid = System.Guid;

namespace LocationServiceTest;

public class LocationServiceControllerTest
{
	private readonly Mock<ILocationRepository> _mockLocationRepository;
	private readonly LocationController _locationController;
	
	public LocationServiceControllerTest()
	{
		var getLocationHandlerMockLogger = new Mock<ILogger<GetLocationHandler>>();
		var addLocationHandlerMockLogger = new Mock<ILogger<AddLocationHandler>>();
		var updateLocationHandlerMockLogger = new Mock<ILogger<UpdateLocationHandler>>();
		_mockLocationRepository = new Mock<ILocationRepository>();
		MapperConfiguration configuration = new MapperConfiguration(cfg =>
		{
			cfg.AddProfile<LocationRequestToLocationMapping>();
		});
		IMapper? mapper = configuration.CreateMapper();

		
		GetLocationHandler getLocationHandler = new (_mockLocationRepository.Object, getLocationHandlerMockLogger.Object);
		AddLocationHandler addLocationHandler = new (_mockLocationRepository.Object, mapper, addLocationHandlerMockLogger.Object);
		UpdateLocationHandler updateLocationHandler = new (_mockLocationRepository.Object, updateLocationHandlerMockLogger.Object);
		_locationController = new (getLocationHandler, addLocationHandler, updateLocationHandler);
	}

	[Test]
	public void TestAddLocation_WhenNewLocationAdded_ShouldReturnNewAddedLocation()
	{
		// Arrange
		LocationRequest locationRequest = new LocationRequest
		{
			street = "street",
			city = "city",
			state = "state",
			postalCode = "postalCode",
			buildingName = "buildingName",
		};
		_mockLocationRepository.Setup(obj => obj.AddLocation(It.IsAny<Location>())).ReturnsAsync((Location location) => location);
		
		// Act
		ActionResult result = _locationController.AddLocation(locationRequest).Result;
		
		// Assert
		Assert.That(result, Is.Not.Null);
		OkObjectResult? okResult = result as OkObjectResult;
		Assert.That(okResult, Is.Not.Null);
		Location? actualLocation = okResult.Value as Location;
		
		Assert.That(actualLocation, Is.Not.Null);
		Assert.That(locationRequest.buildingName, Is.EqualTo(actualLocation.BuildingName));
		Assert.That(locationRequest.city, Is.EqualTo(actualLocation.City));
		Assert.That(locationRequest.street, Is.EqualTo(actualLocation.Street));
		Assert.That(locationRequest.state, Is.EqualTo(actualLocation.State));
		Assert.That(locationRequest.postalCode, Is.EqualTo(actualLocation.PostalCode));
		Assert.That(actualLocation.LocationId, Is.InstanceOf<Guid>());
	}
	
	[Test]
	public void TestGetLocation_WhenInValidCity_ShouldReturnBadRequest()
	{
		// Arrange		
		var city = string.Empty;
		
		// Act
		ActionResult result = _locationController.GetLocationByCity(city).Result;
		
		// Assert
		Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
		BadRequestObjectResult? badResult = result as BadRequestObjectResult;
		Assert.That(badResult, Is.Not.Null);
		Assert.AreEqual("City name cannot be empty.", badResult.Value.ToString());
	}

	[Test]
	public void TestGetLocation_WhenValidCity_ShouldReturnLocationList()
	{
		// Arrange
		const string city = "city";
		List<Location> locations =
		[
			new()
			{
				City = city,
				State = "state",
				Street = "street",
				PostalCode = "postalCode",
				BuildingName = "buildingName"
			}
		];
		
		_mockLocationRepository.Setup(obj => obj.GetAllLocationsByCity(It.IsAny<string>())).ReturnsAsync(locations);
		
		// Act
		ActionResult result = _locationController.GetLocationByCity(city).Result;
		
		// Assert
		Assert.That(result, Is.Not.Null);
		Assert.That(result, Is.InstanceOf<OkObjectResult>());
		
		var okResult = result as OkObjectResult;
		
		Assert.That(okResult, Is.Not.Null);
		
		var actualLocations = okResult.Value as List<Location>;
		
		Assert.That(actualLocations, Is.Not.Null);
		
		Assert.That(actualLocations.Count, Is.EqualTo(1));
		
		Assert.That(actualLocations[0].City, Is.EqualTo(city));
	}

	[Test]
	public void TestUpdateLocation_WhenInValidCity_ShouldReturnNotFound()
	{
		// Arrange
		Guid locationId = Guid.NewGuid();
		LocationRequest locationRequest = new ()
		{
			street = "street",
			city = "city",
			state = "state",
			postalCode = "postalCode",
			buildingName = "buildingName",
		};
		var exceptionMessage = $"No Location for ID: `{locationId}` found.";
		_mockLocationRepository.Setup(obj => obj.GetLocationById(locationId)).Throws(new LocationNotFoundException(exceptionMessage));

		// Act
		ActionResult result = _locationController.UpdateLocation(locationId, locationRequest).Result;
		
		// Assert
		Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
		NotFoundObjectResult? notFoundResult = result as NotFoundObjectResult;
		Assert.That(notFoundResult, Is.Not.Null);
		Assert.AreEqual(exceptionMessage, notFoundResult.Value?.ToString());

	}
}