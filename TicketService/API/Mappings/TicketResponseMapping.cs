using AutoMapper;
using TicketService.API.DTOs;
using TicketService.Domain.Entities;

namespace TicketService.API.Mappings;

public class TicketResponseMapping : Profile
{
	public TicketResponseMapping()
	{
		CreateMap<Ticket, TicketResponse>()
			.ForMember(dest => dest.ticketId, opt => opt.MapFrom(src => src.TicketId))
			.ForMember(dest => dest.bookingId, opt => opt.MapFrom(src => src.BookingId))
			.ForMember(dest => dest.ticketStatus, opt => opt.MapFrom(src => src.Status.ToString()));
	}
}