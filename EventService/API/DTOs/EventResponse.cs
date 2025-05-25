namespace EventService.API.DTOs;

public class EventResponse
{
	public Guid EventId { get; set; } = Guid.NewGuid();
	public string Name { get; set; }
	public string Timing { get; set; }
	public string Address { get; set; }
}