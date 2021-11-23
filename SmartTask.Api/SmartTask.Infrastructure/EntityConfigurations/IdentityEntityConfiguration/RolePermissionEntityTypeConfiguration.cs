using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartTask.Domain.AggregatesModel.RoleAggregate;
using SmartTask.Infrastructure.Database;
namespace SmartTask.Infrastructure.EntityConfigurations.IdentityEntityConfiguration
{
    public class RolePermissionEntityTypeConfiguration : IEntityTypeConfiguration<RolePermission>
    {
        public void Configure(EntityTypeBuilder<RolePermission> rolePermissionConfiguration)
        {
            rolePermissionConfiguration.ToTable("role_permissions");

            rolePermissionConfiguration.HasKey(o => o.Id);

            rolePermissionConfiguration.HasIndex(o => new { o.RoleId, o.PermissionId }).IsUnique();

            var parameterValuesNavigation = rolePermissionConfiguration.Metadata.FindNavigation(nameof(RolePermission.ParameterValues));
            parameterValuesNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
