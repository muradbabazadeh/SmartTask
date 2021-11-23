using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartTask.Domain.AggregatesModel.RoleAggregate;
using SmartTask.Infrastructure.Database;
namespace SmartTask.Infrastructure.EntityConfigurations.IdentityEntityConfiguration
{
    public class RoleEntityTypeConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> roleConfiguration)
        {
            roleConfiguration.ToTable("roles");

            roleConfiguration.HasKey(o => o.Id);

            roleConfiguration.HasIndex(o => o.Name).IsUnique();

            roleConfiguration.Property(o => o.Name).HasMaxLength(100).IsRequired();

            var permissionsNavigation = roleConfiguration.Metadata.FindNavigation(nameof(Role.Permissions));
            permissionsNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
