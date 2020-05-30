using AutoMapper;
using GalaxyWebApi.Models;

namespace GalaxyWebApi.Mappings
{
    public class CarsViewMappings : Profile
    {
        public CarsViewMappings()
        {
            CreateMap<GalaxyCore.Models.Car, Car>()
                .ForMember(d => d.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(d => d.ModelName, opt => opt.MapFrom(src => src.ModelName))
                .ForMember(d => d.CarType, opt => opt.MapFrom(src => src.CarType))
                .ForMember(d => d.CreatedOn, opt => opt.MapFrom(src => src.CreatedOn))
                .ForMember(d => d.ModifiedOn, opt => opt.MapFrom(src => src.ModifiedOn));

            CreateMap<Car, GalaxyCore.Models.Car>()
                .ForMember(d => d.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(d => d.ModelName, opt => opt.MapFrom(src => src.ModelName))
                .ForMember(d => d.CarType, opt => opt.MapFrom(src => src.CarType))
                .ForMember(d => d.CreatedOn, opt => opt.MapFrom(src => src.CreatedOn))
                .ForMember(d => d.ModifiedOn, opt => opt.MapFrom(src => src.ModifiedOn));
        }
    }
}
