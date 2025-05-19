using AutoMapper;
using LocationService.API.Controllers;
using LocationService.API.DTOs;
using LocationService.Application.Interfaces;
using LocationService.Application.Mappings;
using LocationService.Application.UseCases;
using LocationService.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace LocationServiceTest;

public class LocationServiceControllerTest
{
	private readonly Mock<ILogger<GetLocationHandler>> _getLocationHandlerMockLogger;
	private readonly Mock<ILocationRepository> mockLocationRepository;
	private readonly LocationController locationController;
	
	public LocationServiceControllerTest()
	{
		_getLocationHandlerMockLogger = new Mock<ILogger<GetLocationHandler>>();
		var addLocationHandlerMockLogger = new Mock<ILogger<AddLocationHandler>>();
		var updateLocationHandlerMockLogger = new Mock<ILogger<UpdateLocationHandler>>();
		mockLocationRepository = new Mock<ILocationRepository>();
		var configuration = new MapperConfiguration(cfg =>
		{
			cfg.AddProfile<LocationRequestToLocationMapping>();
		});
		var mapper = configuration.CreateMapper();

		
		GetLocationHandler getLocationHandler = new (mockLocationRepository.Object, _getLocationHandlerMockLogger.Object);
		AddLocationHandler addLocationHandler = new (mockLocationRepository.Object, mapper, addLocationHandlerMockLogger.Object);
		UpdateLocationHandler updateLocationHandler = new (mockLocationRepository.Object, updateLocationHandlerMockLogger.Object);
		locationController = new (getLocationHandler, addLocationHandler, updateLocationHandler);
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
		mockLocationRepository.Setup(obj => obj.AddLocation(It.IsAny<Location>())).ReturnsAsync((Location location) => location);
		
		// Act
		ActionResult result = locationController.AddLocation(locationRequest).Result;
		
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
}