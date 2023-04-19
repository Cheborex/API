using API_Completa.Modelos;
using API_Completa.Modelos.Dto;
using AutoMapper;
using System.Runtime.InteropServices;

namespace API_Completa
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<Api, ApiDto>(); // Es lo mismo pero se simplifica el codigo. Es mejor usar ReverseMap
            CreateMap<ApiDto, Api>(); // Esto se utiliza para ahorar codigo en el controlador. Es una buena practica

            CreateMap<Api, ApiCreateDto>().ReverseMap();
            CreateMap<Api, ApiUpdateDto>().ReverseMap();

            CreateMap<NumeroApi, NumeroApiDto>().ReverseMap();
            CreateMap<NumeroApi, NumeroApiCreateDto>().ReverseMap();
            CreateMap<NumeroApi, NumeroApiUpdateDto>().ReverseMap();
        }
        
    }
}
