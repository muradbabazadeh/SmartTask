using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartTask.Infrastructure.Constants;
using SmartTask.Infrastructure.Database;
using SmartTask.Infrastructure.Idempotency;

namespace SmartTask.Infrastructure.EntityConfigurations.AuditEntityConfiguration
{
    internal class ClientRequestEntityTypeConfiguration
      : IEntityTypeConfiguration<ClientRequest>
    {
        public void Configure(EntityTypeBuilder<ClientRequest> requestConfiguration)
        {
            requestConfiguration.ToTable("requests");

            requestConfiguration.HasKey(cr => cr.Id);
            requestConfiguration.HasIndex(cr => cr.Key).IsUnique();
            requestConfiguration.Property(cr => cr.Name).HasColumnName("name").IsRequired();
            requestConfiguration.Property(cr => cr.Time).HasColumnName("time").HasDefaultValue("10-03-2021");
        }
    }
}
