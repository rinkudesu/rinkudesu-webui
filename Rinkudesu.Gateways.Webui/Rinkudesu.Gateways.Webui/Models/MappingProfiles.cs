using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using Rinkudesu.Gateways.Clients.Links;
using Rinkudesu.Gateways.Clients.Tags;

namespace Rinkudesu.Gateways.Webui.Models
{
    [ExcludeFromCodeCoverage]
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<LinkDto, LinkIndexViewModel>();
            CreateMap<TagDto, TagIndexViewModel>();
            CreateMap<LinkIndexQueryModel, LinkQueryDto>()
                //todo: remove this mapping once multiple tags can be selected
                .ForMember(dest => dest.TagIds, opt => opt.MapFrom(src => src.TagIds == null ? null : new []{ src.TagIds }));
        }
    }
}
