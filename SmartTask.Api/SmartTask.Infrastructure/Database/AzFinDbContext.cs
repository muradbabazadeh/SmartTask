using SmartTask.Domain.AggregatesModel.PermissionAggregate;
using SmartTask.Domain.AggregatesModel.RoleAggregate;
using SmartTask.Domain.AggregatesModel.UserAggregate;
using SmartTask.Infrastructure.EntityConfigurations.AuditEntityConfiguration;
using SmartTask.Infrastructure.EntityConfigurations.IdentityEntityConfiguration;
using SmartTask.SharedKernel.Domain.Seedwork;
using SmartTask.SharedKernel.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SmartTask.Infrastructure.Database
{
   public class SmartTaskDbContext : DbContext, IUnitOfWork
    {
        public const string IDENTITY_SCHEMA = "Identity";
        public const string DEFAULT_SCHEMA = "dbo";

        private readonly IMediator _mediator;

        public SmartTaskDbContext(DbContextOptions options, IMediator mediator) : base(options)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public SmartTaskDbContext()
        {

        }

        public DbSet<User> Users { get; private set; }
        public DbSet<Role> Roles { get; private set; }
        public DbSet<UserRole> UserRoles { get; private set; }
        public DbSet<UserPermission> UserPermisson { get; private set; }
        public DbSet<UserPermissionParameterValue> UserPermissonParametrValues { get; private set; }
        public DbSet<RolePermission> RolePermissons { get; private set; }
        public DbSet<RolePermissionParameterValue> RolePermissonParametrValues { get; private set; }
        public DbSet<Permission> Permissions { get; private set; }

        public DbSet<PermissionParameter> PermissionParametrs { get; private set; }


        public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            await _mediator.DispatchDomainEventsAsync(this);

            await SaveChangesAsync(true, cancellationToken);

            return true;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ClientRequestEntityTypeConfiguration());
            // Identity
            modelBuilder.ApplyConfiguration(new RoleEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new RolePermissionEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ParameterValueEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new PermissionParameterEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new UserEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new UserRoleEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new UserPermissionEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new UserPermissionParameterValueEntityTypeConfiguration());

            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.ClientSetNull;
            }
        }
    }
}
