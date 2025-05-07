using System.ComponentModel.DataAnnotations;
using TicketService.API.Validations;

namespace TicketService.API.DTOs;

public class TicketRequest
{
	[Required(ErrorMessage = "Booking ID is required.")]
	[NotEmptyGuid(ErrorMessage = "Booking ID cannot be empty.")]
	public Guid bookingId { get; set; }
}