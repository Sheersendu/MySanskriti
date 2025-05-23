namespace EventService.Application.DTOs;

public class LocationDTO
{
	public Guid LocationId { get; set; }
	public string BuildingName { get; set; }
	public string Street { get; set; }
	public string City { get; set; }
	public string State { get; set; }
	public string PostalCode { get; set; }
}