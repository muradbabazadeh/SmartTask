using SmartTask.Domain.AggregatesModel.RoleAggregate;
using SmartTask.Domain.AggregatesModel.UserAggregate;
using SmartTask.Identity.ViewModels;
using SmartTask.Identity.ViewModels.FilterModel;
using SmartTask.Infrastructure.Constants;
using SmartTask.SharedKernel.Infrastructure.Queries;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartTask.Identity.Queries
{
    public interface IUserQueries : IQuery
    {
        Task<IDictionary<string, PermissionDTO>> GetPermissionsAsync(int userId);

        Task<User> GetUserWithPermissionsAsync(int userId);

        Task<User> FindByNameAsync(string userName);

        Task<User> FindByEmailAsync(string email);

        Task<User> FindAsync(int userId);

        Task<UserProfileDTO> GetUserProfileAsync(int userId);
        Task<UserProfileDTO> GetUserByIdAsync(int userId);

        Task<User> GetUserEntityAsync(int? userId);

        Task<UserAllProfileDto> GetAllAsync(LoadMoreDTO loadMore);

        Task<UserAllProfileDto> FilterAsync(LoadMoreDTO loadMore, UserFilterDto userFilter);
        Task<string> GetExistingUser(string userName);

        Task<Role> GetRoleAsyncById(int? id);

        Task<IEnumerable<PermissionAllDTO>> GetAllPermission();
        Task<IEnumerable<RoleAllDTO>> GetAllRole();
        Task<RoleAllDTO> GetByIdRole(int id);
        Task<IEnumerable<PermissionByUserIdDTO>> GetPermissionByUserIdAsync(int userId);
    }
}
