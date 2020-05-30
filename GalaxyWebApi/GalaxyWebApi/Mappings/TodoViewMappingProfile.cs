using AutoMapper;
using GalaxyWebApi.Models;

namespace GalaxyWebApi.Mappings
{
    public class TodoViewMappings : Profile
    {
        public TodoViewMappings()
        {
            CreateMap<GalaxyCore.Models.Todo, Todo>()
                .ForMember(d => d.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(d => d.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(d => d.Completed, opt => opt.MapFrom(src => src.Completed));

            CreateMap<Todo, GalaxyCore.Models.Todo>()
                .ForMember(d => d.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(d => d.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(d => d.Completed, opt => opt.MapFrom(src => src.Completed));
        }
    }
}
