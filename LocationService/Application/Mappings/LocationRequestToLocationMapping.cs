using AutoMapper;
using LocationService.API.DTOs;
using LocationService.Domain.Entities;

namespace LocationService.Application.Mappings;

public class LocationRequestToLocationMapping : Profile
{
	public LocationRequestToLocationMapping()
	{
		CreateMap<LocationRequest, Location>()
			.ForMember(dest => dest.City, opt => opt.MapFrom(src => src.city))
			.ForMember(dest => dest.State, opt => opt.MapFrom(src => src.state))
			.ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.street))
			.ForMember(dest => dest.PostalCode, opt => opt.MapFrom(src => src.postalCode));
	}
}