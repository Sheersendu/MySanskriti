using System.Text.Json.Serialization;
using EventService.Domain.Enums;

namespace EventService.Domain.Entities;

public class Event
{
	public Guid EventId { get; set; } = Guid.NewGuid();
	public string Name { get; set; }
	public string Description { get; set; }
	public string Timing { get; set; }
	public string Type { get; set; }
	public Guid LocationId { get; set; }
	[JsonIgnore]
	public string Status { get; set; }
	
	public Event(){}

	public Event(string name, string description, string timing, string type, Guid locationId, string status)
	{
		Name = name;
		Description = description;
		Timing = timing;
		Type = type;
		LocationId = locationId;
		Status = status;
	}

	public void UpdateDetails(string name, string description, string timing, string type, Guid locationId)
	{
		Name = name;
		Description = description;
		Timing = timing;
		Type = type;
		LocationId = locationId;
	}

	public void Cancel()
	{
		Status = EventStatus.CANCELLED.ToString();
	}
}
