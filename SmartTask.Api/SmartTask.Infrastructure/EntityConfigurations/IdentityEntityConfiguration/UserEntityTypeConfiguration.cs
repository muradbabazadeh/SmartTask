using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartTask.Domain.AggregatesModel.UserAggregate;
using SmartTask.Infrastructure.Database;
namespace SmartTask.Infrastructure.EntityConfigurations.IdentityEntityConfiguration
{
    public class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> userConfiguration)
        {
            userConfiguration.ToTable("users");

            userConfiguration.HasKey(o => o.Id);
            userConfiguration.Property(o => o.Id).HasColumnName("id");
            var permissionsNavigation = userConfiguration.Metadata.FindNavigation(nameof(User.Permissions));
            permissionsNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);

            var rolesNavigation = userConfiguration.Metadata.FindNavigation(nameof(User.Roles));
            rolesNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);
            userConfiguration.Property(o => o.FirstName).HasColumnName("first_name").HasMaxLength(100).IsRequired();
            userConfiguration.Property(o => o.Email).HasColumnName("email").HasMaxLength(100).IsRequired();
            userConfiguration.Property(o => o.PasswordHash).HasColumnName("password_hash").IsRequired();
        }
    }
}
