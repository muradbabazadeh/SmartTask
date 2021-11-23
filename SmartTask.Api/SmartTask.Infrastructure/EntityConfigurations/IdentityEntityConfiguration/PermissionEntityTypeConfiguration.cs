using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartTask.Domain.AggregatesModel.PermissionAggregate;
using SmartTask.Infrastructure.Database;

namespace SmartTask.Infrastructure.EntityConfigurations.IdentityEntityConfiguration
{
    public class PermissionEntityTypeConfiguration : IEntityTypeConfiguration<Permission>
    {
        public void Configure(EntityTypeBuilder<Permission> permissionConfiguration)
        {
            permissionConfiguration.ToTable("permissions");

            permissionConfiguration.HasKey(o => o.Id);
         
            permissionConfiguration.HasIndex(o => o.Name).IsUnique();

            permissionConfiguration.Property(o => o.Name)
                .HasMaxLength(50)
                .IsRequired();

            permissionConfiguration.Property(o => o.Description)
                .HasMaxLength(255)
                .IsRequired();

            var parametersNavigation = permissionConfiguration.Metadata.FindNavigation(nameof(Permission.Parameters));
            parametersNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
