using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartTask.Domain.AggregatesModel.RoleAggregate;
using SmartTask.Infrastructure.Database;

namespace SmartTask.Infrastructure.EntityConfigurations.IdentityEntityConfiguration
{
    public class ParameterValueEntityTypeConfiguration : IEntityTypeConfiguration<RolePermissionParameterValue>
    {
        public void Configure(EntityTypeBuilder<RolePermissionParameterValue> parameterValueConfiguration)
        {
            parameterValueConfiguration.ToTable("role_permission_parameter_values");

            parameterValueConfiguration.HasKey(o => o.Id);

           
            parameterValueConfiguration.Property(o => o.Value).HasMaxLength(255).IsRequired();
        }
    }
}
