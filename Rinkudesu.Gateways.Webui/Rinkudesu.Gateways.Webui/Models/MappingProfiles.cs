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
            CreateMap<LinkIndexQueryModel, LinkQueryDto>();
            CreateMap<TagIndexQueryModel, TagQueryDto>();
            CreateMap<LinkIndexViewModel, LinkDto>();
        }
    }
}
