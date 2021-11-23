using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SmartTask.Domain.AggregatesModel.PermissionAggregate;
using SmartTask.Domain.AggregatesModel.UserAggregate;
using SmartTask.Infrastructure;
using SmartTask.Infrastructure.Database;
using SmartTask.SharedKernel.Domain.Seedwork;
using SmartTask.SharedKernel.Infrastructure;
using Polly;
using Polly.Retry;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SmartTask.Core.Api.Infrastructure.DBSeed
{
    public class SmartTaskDbContextSeed
    {
        public async Task SeedAsync(
            SmartTaskDbContext context,
            IWebHostEnvironment env,
            IOptions<SmartTaskSettings> settings,
            ILogger<SmartTaskDbContextSeed> logger)
        {
            var policy = CreatePolicy(logger, nameof(SmartTaskDbContextSeed));

            using (context)
            {
                await policy.ExecuteAsync(async () =>
                {


                    // Seed Permissions
                    await SeedPermissions(context);

                    await context.SaveChangesAsync();

                    var systemUser = await context.Users.SingleOrDefaultAsync(u => u.Email == "SmartTask_admin");


                    if (systemUser == null)
                    {
                        var user = new SmartTask.Domain.AggregatesModel.UserAggregate.User
                        ("system.user@SmartTask.az",PasswordHasher.HashPassword("SmartTask_crm2021"), false, "The Boss", "SmartTask");

                        
                        var permissions = await context.Permissions.Include(p => p.Parameters).ToListAsync();

                        foreach (var permission in permissions)
                        {
                            user.AddPermission(permission.Id);
                        }

                        await context.Users.AddAsync(user);
                        await context.SaveChangesAsync();


                        var adminUser = await context.Users.Include(p => p.Permissions).ThenInclude(p => p.Permission).ThenInclude(p => p.Parameters).FirstOrDefaultAsync(p => p.Email == "SmartTask_admin");

                        foreach (var permission in adminUser.Permissions)
                        {
                            var scope = await context.PermissionParametrs.FirstOrDefaultAsync(p => p.PermissionId == permission.PermissionId && p.DefaultValue == "All");

                            if (scope != null)
                            {
                                var userPermissionParametr = new UserPermissionParameterValue();

                                userPermissionParametr.AddToInfo(scope.Id, scope.DefaultValue, permission.Id);
                                await context.UserPermissonParametrValues.AddAsync(userPermissionParametr);
                            }

                           
                        }


                        await context.SaveChangesAsync();


                    }
                });
            }
        }

        private async Task SeedPermissions(SmartTaskDbContext context)
        {
            string permissionsJson = File.ReadAllText("./Infrastructure/DBSeed/permission.json");
            var permissions = JsonConvert.DeserializeObject<List<PermissionSeedDTO>>(permissionsJson);
            foreach (PermissionSeedDTO permissionDTO in permissions)
            {
                await SeedPermission(context, permissionDTO);
            }

            await context.SaveChangesAsync();
        }

        private async Task SeedPermission(SmartTaskDbContext context, PermissionSeedDTO permissionDTO)
        {
            var permission = await context.Permissions.Include(p => p.Parameters).SingleOrDefaultAsync(p => p.Id == permissionDTO.Id);
            PermissionCode permissionCode = Enumeration.FromValue<PermissionCode>(permissionDTO.Id);

            if (permission == null)
            {
                permission = new Permission();
                permission.SetDetails(permissionCode, permissionDTO.Description);
                foreach (ParameterSeedDTO parameterDTO in permissionDTO.Parameters)
                {
                    PermissionParameterCode parameterCode = Enumeration.FromValue<PermissionParameterCode>(parameterDTO.Id);
                    var parameter = new PermissionParameter();
                    parameter.SetDetails(parameterCode, parameterDTO.Description, parameterDTO.Type, parameterDTO.DefaultValue);
                    permission.AddParameter(parameter);
                }

                await context.Permissions.AddAsync(permission);
            }
            else
            {
                permission.SetDetails(permissionCode, permissionDTO.Description);
                foreach (ParameterSeedDTO parameterDTO in permissionDTO.Parameters)
                {
                    PermissionParameterCode parameterCode = Enumeration.FromValue<PermissionParameterCode>(parameterDTO.Id);
                    var parameter = permission.Parameters.SingleOrDefault(p => p.Id == parameterCode.Id);
                    if (parameter == null)
                    {
                        parameter = new PermissionParameter();
                        parameter.SetDetails(parameterCode, parameterDTO.Description, parameterDTO.Type, parameterDTO.DefaultValue);
                        permission.AddParameter(parameter);
                    }
                    else
                    {
                        parameter.SetDetails(parameterCode, parameterDTO.Description, parameterDTO.Type, parameterDTO.DefaultValue);
                    }
                }

                context.Permissions.Update(permission);
            }
        }

        private AsyncRetryPolicy CreatePolicy(ILogger<SmartTaskDbContextSeed> logger, string prefix, int retries = 3)
        {
            return Policy.Handle<SqlException>().WaitAndRetryAsync(
                retries,
                retry => TimeSpan.FromSeconds(5),
                (exception, timeSpan, retry, ctx) =>
                {
                    logger.LogTrace(
                        $"[{prefix}] Exception {exception.GetType().Name} with message ${exception.Message} detected on attempt {retry} of {retries}");
                }
            );
        }
    }
}
