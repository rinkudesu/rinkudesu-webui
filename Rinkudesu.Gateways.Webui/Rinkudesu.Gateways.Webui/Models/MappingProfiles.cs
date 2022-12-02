using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using Rinkudesu.Gateways.Clients.Links;

namespace Rinkudesu.Gateways.Webui.Models
{
    [ExcludeFromCodeCoverage]
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<LinkDto, LinkIndexViewModel>();
        }
    }
}
