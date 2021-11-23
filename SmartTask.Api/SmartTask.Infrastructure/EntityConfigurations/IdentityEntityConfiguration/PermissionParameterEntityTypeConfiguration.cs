using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartTask.Domain.AggregatesModel.PermissionAggregate;
using SmartTask.Infrastructure.Database;
namespace SmartTask.Infrastructure.EntityConfigurations.IdentityEntityConfiguration
{
    public class PermissionParameterEntityTypeConfiguration : IEntityTypeConfiguration<PermissionParameter>
    {
        public void Configure(EntityTypeBuilder<PermissionParameter> permissionParameterConfiguration)
        {
            permissionParameterConfiguration.ToTable("permission_parameters");

            permissionParameterConfiguration.HasKey(o => o.Id);
            permissionParameterConfiguration.Property(o => o.Name).HasMaxLength(128).IsRequired();
            permissionParameterConfiguration.Property(o => o.Description).HasMaxLength(255).IsRequired();
            permissionParameterConfiguration.Property(o => o.DefaultValue).HasMaxLength(255).IsRequired();
            permissionParameterConfiguration.Property(o => o.Type).HasMaxLength(64).IsRequired();
        }
    }
}
