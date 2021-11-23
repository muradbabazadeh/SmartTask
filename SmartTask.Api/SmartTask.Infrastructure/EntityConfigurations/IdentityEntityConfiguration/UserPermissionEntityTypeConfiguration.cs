using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartTask.Domain.AggregatesModel.UserAggregate;
using SmartTask.Infrastructure.Database;
namespace SmartTask.Infrastructure.EntityConfigurations.IdentityEntityConfiguration
{
    public class UserPermissionEntityTypeConfiguration : IEntityTypeConfiguration<UserPermission>
    {
        public void Configure(EntityTypeBuilder<UserPermission> userPermissionConfiguration)
        {
            userPermissionConfiguration.ToTable("user_permissions");

            userPermissionConfiguration.HasKey(o => o.Id);

            userPermissionConfiguration.HasIndex(o => new { o.UserId, o.PermissionId }).IsUnique();

            var parameterValuesNavigation = userPermissionConfiguration.Metadata.FindNavigation(nameof(UserPermission.ParameterValues));
            parameterValuesNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
