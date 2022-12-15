using AutoMapper;
using ClinicService.Data;
using ClinicServiceNamespace;

namespace ClinicService.Api.Mapper;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<Client, ClientResponse>()
            .ForMember(dst => dst.ClientId, opt => opt.MapFrom(src => src.Id))
            .ReverseMap();
        CreateMap<CreateClientRequest, Client>();
        CreateMap<UpdateClientRequest, Client>();
    }
}
