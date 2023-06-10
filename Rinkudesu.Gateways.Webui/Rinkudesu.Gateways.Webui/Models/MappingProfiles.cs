using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using Rinkudesu.Gateways.Clients.Identity;
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
            CreateMap<PasswordChangeViewModel, PasswordChangeDto>();
            CreateMap<DeleteAccountViewModel, AccountDeleteDto>();
            CreateMap<RegisterAccountViewModel, RegisterAccountDto>();
            CreateMap<ForgotPasswordViewModel, ForgotPasswordDto>();
            CreateMap<ChangeForgottenPasswordViewModel, ChangeForgottenPasswordDto>();
            CreateMap<ChangeEmailViewModel, ChangeEmailDto>();
            CreateMap<UserAdminIndexQueryModel, UserAdminIndexQueryDto>();
            CreateMap<UserAdminDetailsDto, UserAdminDetailsViewModel>();
        }
    }
}
