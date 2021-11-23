using Autofac;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using SmartTask.Domain.AggregatesModel.RoleAggregate;
using SmartTask.Domain.AggregatesModel.UserAggregate;
using SmartTask.Identity;
using SmartTask.Identity.Auth;
using SmartTask.Infrastructure;
using SmartTask.Infrastructure.Idempotency;
using SmartTask.Infrastructure.Identity;
using SmartTask.Infrastructure.Repositories;
using SmartTask.Shared;
using SmartTask.User;
using System.Collections.Generic;

namespace SmartTask.Core.Api.Infrastructure.AutofacModules
{
    public class ApplicationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            AutofacHelper.RegisterCqrsTypes<SmartTaskSharedModule>(builder);
            AutofacHelper.RegisterCqrsTypes<IdentityModule>(builder);
            AutofacHelper.RegisterCqrsTypes<UsrModule>(builder);
            AutofacHelper.RegisterCqrsTypes<ApplicationModule>(builder);
            AutofacHelper.RegisterCqrsTypes<ApplicationModule>(builder);

            // Services

            // Repositories
            builder.RegisterType<UserRepository>()
                .As<IUserRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<ClaimsManager>()
                .As<IClaimsManager>()
                .InstancePerLifetimeScope();

            builder.RegisterType<UserManager>()
                .As<IUserManager>()
                .InstancePerLifetimeScope();

            builder.RegisterType<RequestManager>()
                .As<IRequestManager>()
                .InstancePerLifetimeScope();

            builder.RegisterType<RoleRepository>()
              .As<IRoleRepository>()
              .InstancePerLifetimeScope();


         
            // AutoMapper
            AutofacHelper.RegisterAutoMapperProfiles<IdentityModule>(builder);
            AutofacHelper.RegisterAutoMapperProfiles<ApplicationModule>(builder);
            AutofacHelper.RegisterAutoMapperProfiles<UsrModule>(builder);
            AutofacHelper.RegisterAutoMapperProfiles<SmartTaskSharedModule>(builder);

            builder.Register(ctx =>
            {
                var mapperConfiguration = new MapperConfiguration(cfg =>
                {
                    foreach (var profile in ctx.Resolve<IList<Profile>>()) cfg.AddProfile(profile);
                });
                return mapperConfiguration.CreateMapper();
            })
                .As<IMapper>()
                .SingleInstance();
        }
    }
}
