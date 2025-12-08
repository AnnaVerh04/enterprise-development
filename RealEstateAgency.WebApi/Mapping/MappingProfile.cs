using AutoMapper;
using RealEstateAgency.Domain.Models;
using RealEstateAgency.WebApi.DTOs;

namespace RealEstateAgency.WebApi.Mapping;

/// <summary>
/// Профиль AutoMapper для маппинга сущностей и DTO
/// </summary>
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Counterparty mappings
        CreateMap<Counterparty, CounterpartyDto>();
        CreateMap<CreateCounterpartyDto, Counterparty>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());
        CreateMap<UpdateCounterpartyDto, Counterparty>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());

        // RealEstateProperty mappings
        CreateMap<RealEstateProperty, RealEstatePropertyDto>();
        CreateMap<CreateRealEstatePropertyDto, RealEstateProperty>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());
        CreateMap<UpdateRealEstatePropertyDto, RealEstateProperty>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());

        // Request mappings
        CreateMap<Request, RequestDto>()
            .ForMember(dest => dest.CounterpartyId, opt => opt.MapFrom(src => src.Counterparty.Id))
            .ForMember(dest => dest.PropertyId, opt => opt.MapFrom(src => src.Property.Id));
    }
}
