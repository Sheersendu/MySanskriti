namespace LocationService.Domain.Entities;

public class Location
{
	public Guid LocationId { get; set; } = Guid.NewGuid();
	public required string BuildingName { get; set; }
	public required string Street { get; set; }
	public required string City { get; set; }
	public required string State { get; set; }
	public required string PostalCode { get; set; }

	public Location() { }
	
	public Location(string buildingName, string street, string city, string state, string postalCode)
	{
		BuildingName = buildingName;
		Street = street;
		City = city;
		State = state;
		PostalCode = postalCode;
	}
	
	public void UpdateDetails(string buildingName, string street, string city, string state, string postalCode)
	{
		BuildingName = buildingName;
		Street = street;
		City = city;
		State = state;
		PostalCode = postalCode;
	}
}