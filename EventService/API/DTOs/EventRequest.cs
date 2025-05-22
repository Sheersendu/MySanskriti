using System.ComponentModel.DataAnnotations;

namespace EventService.API.DTOs;

public class EventRequest
{
	[Required(ErrorMessage = "Name is required.")]
	public string Name { get; set; }
	
	[Required(ErrorMessage = "Description is required.")]
	[StringLength(200, ErrorMessage = "Description cannot exceed 200 characters")]
	public string Description { get; set; }
	
	[Required(ErrorMessage = "Event timing is required.")]
	public DateTime Timing { get; set; }
	
	[Required(ErrorMessage = "Event Type is required.")]
	public string Type { get; set; }
	
	[Required(ErrorMessage = "LocationId is required.")]
	public Guid LocationId { get; set; }
}