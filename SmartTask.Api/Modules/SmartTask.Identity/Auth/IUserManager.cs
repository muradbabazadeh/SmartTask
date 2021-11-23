using SmartTask.Domain.AggregatesModel.UserAggregate;
using SmartTask.Identity.ViewModels;
using System;
using System.Threading.Tasks;
namespace SmartTask.Identity.Auth
{
    public interface IUserManager
    {
        int GetCurrentUserId();

        string GetCurrentUserName();

        Task<User> GetCurrentUser();

        (string token, DateTime expiresAt) GenerateJwtToken(User user);

        Task<PermissionDTO> GetPermissionAsync(string permissionName);
    }
}
