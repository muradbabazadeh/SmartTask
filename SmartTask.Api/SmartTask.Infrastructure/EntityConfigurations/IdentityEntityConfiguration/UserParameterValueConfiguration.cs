using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartTask.Domain.AggregatesModel.UserAggregate;
using SmartTask.Infrastructure.Database;
namespace SmartTask.Infrastructure.EntityConfigurations.IdentityEntityConfiguration
{
    public class UserPermissionParameterValueEntityTypeConfiguration : IEntityTypeConfiguration<UserPermissionParameterValue>
    {
        public void Configure(EntityTypeBuilder<UserPermissionParameterValue> parameterValueConfiguration)
        {
            parameterValueConfiguration.ToTable("user_permission_parameter_values");

            parameterValueConfiguration.HasKey(o => o.Id);

            parameterValueConfiguration.Property(o => o.Value).HasMaxLength(255).IsRequired();
        }
    }
}
