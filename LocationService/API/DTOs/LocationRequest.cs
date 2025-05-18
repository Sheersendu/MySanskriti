using System.ComponentModel.DataAnnotations;

namespace LocationService.API.DTOs;

public class LocationRequest
{
	[Required(ErrorMessage = "Street is required.")]
	[StringLength(100, ErrorMessage = "Street name should be within 5 to 100 characters.", MinimumLength = 5)]
	public required string street { get; set; }
	
	[Required(ErrorMessage = "City is required.")]
	[StringLength(100, ErrorMessage = "City name should be within 5 to 100 characters.", MinimumLength = 5)]
	public required string city { get; set; }
	
	[Required(ErrorMessage = "State is required.")]
	[StringLength(50, ErrorMessage = "State name should be within 5 to 50 characters.", MinimumLength = 5)]
	public required string state { get; set; }
	
	[Required(ErrorMessage = "Postal code is required.")]
	[StringLength(6, ErrorMessage = "Postal code should be 6 characters.", MinimumLength = 6)]
	public required string postalCode { get; set; }
}