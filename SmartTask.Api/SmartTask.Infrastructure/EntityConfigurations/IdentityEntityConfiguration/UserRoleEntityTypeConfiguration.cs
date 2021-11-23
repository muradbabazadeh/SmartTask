using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartTask.Domain.AggregatesModel.UserAggregate;
using SmartTask.Infrastructure.Database;

namespace SmartTask.Infrastructure.EntityConfigurations.IdentityEntityConfiguration
{
    public class UserRoleEntityTypeConfiguration : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> userRoleConfiguration)
        {
            userRoleConfiguration.ToTable("user_roles");

            userRoleConfiguration.HasKey(o => o.Id);

            userRoleConfiguration.HasIndex(o => new { o.UserId, o.RoleId }).IsUnique();
        }
    }
}
