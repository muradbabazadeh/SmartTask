using AutoMapper;
using SmartTask.Domain.AggregatesModel.PermissionAggregate;
using SmartTask.Domain.AggregatesModel.RoleAggregate;
using SmartTask.Domain.AggregatesModel.UserAggregate;
using SmartTask.Identity.ViewModels;
using System.Linq;

namespace SmartTask.Identity.IdentityProfiles
{
    public class PermissionProfile : Profile
    {
        public PermissionProfile()
        {

            CreateMap<Permission, PermissionDTO>();
            CreateMap<Permission, PermissionAllDTO>()
                  .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Description));

            CreateMap<Role, RoleAllDTO>();
            CreateMap<RolePermission, RolePermissionDTO>()
                 .ForMember(dest => dest.ScopeId, opt => opt.MapFrom(src => src.ParameterValues.FirstOrDefault().PermissionParameterId))
                 .ForMember(dest => dest.PermissionName, opt => opt.MapFrom(src => src.Permission.Name))
                 .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.ParameterValues.FirstOrDefault().Value));

            CreateMap<RolePermissionParameterValue, PermissionParametrForRoleDTO>();
            CreateMap<UserPermissionParameterValue, UserPermissionParametrDTO>();
            CreateMap<PermissionParameter, PermissionParametrAllDTO>()
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                 .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.DefaultValue));

            CreateMap<PermissionParameter, PermissionParameterDTO>();
            CreateMap<UserPermission, PermissionByUserIdDTO>()
                 .ForMember(dest => dest.PermissionName, opt => opt.MapFrom(src => src.Permission.Name));


            CreateMap<RolePermission, PermissionDTO>().ConstructUsing(ConvertRolePermission);
            CreateMap<UserPermission, PermissionDTO>().ConstructUsing(ConvertUserPermission);

            CreateMap<User, UserProfileDTO>()
                 .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.Roles.FirstOrDefault().RoleId))
                 .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Roles.FirstOrDefault().Role.Name))
                .ForMember(dest => dest.Permissions, opt => opt.Ignore());
                
            CreateMap<User, GroupUserDTO>();
        }

        private PermissionDTO ConvertRolePermission(RolePermission rolePermission, ResolutionContext resolutionContext)
        {
            PermissionDTO permissionDTO = resolutionContext.Mapper.Map<PermissionDTO>(rolePermission.Permission);
            foreach (PermissionParameterDTO parameter in permissionDTO.Parameters)
            {
                parameter.Values = rolePermission.ParameterValues.Where(r => r.PermissionParameterId == parameter.Id).Select(v => v.Value).ToList();
            }

            return permissionDTO;
        }

        private PermissionDTO ConvertUserPermission(UserPermission userPermission, ResolutionContext resolutionContext)
        {
            PermissionDTO permissionDTO = resolutionContext.Mapper.Map<PermissionDTO>(userPermission.Permission);
            foreach (PermissionParameterDTO parameter in permissionDTO.Parameters)
            {
                parameter.Values = userPermission.ParameterValues.Where(r => r.PermissionParameterId == parameter.Id).Select(v => v.Value).ToList();
            }

            return permissionDTO;
        }
    }
}
