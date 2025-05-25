using AutoMapper;
using EventService.API.DTOs;
using EventService.Domain.Entities;

namespace EventService.API.Mappings;

public class EventResponseMapping : Profile
{
	public EventResponseMapping()
	{
		CreateMap<Event, EventResponse>()
			.ForMember(dest => dest.EventId, opt => opt.MapFrom(src => src.EventId))
			.ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
			.ForMember(dest => dest.Timing, opt => opt.MapFrom(src => src.Timing))
			.ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address));
	}
}