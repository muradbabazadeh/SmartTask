using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using SmartTask.Core.Api.Infrastructure.AutofacModules;
using SmartTask.Core.Api.Infrastructure.ErrorHandling;
using SmartTask.Core.Api.Infrastructure.Filters;
using SmartTask.Core.Api.Infrastructure.Middlewares;
using SmartTask.Infrastructure;
using SmartTask.Infrastructure.Database;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartTask.Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; private set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
            });

            services.AddControllers();
            services.AddControllers()
                .AddNewtonsoftJson(opt =>
                {
                    opt.SerializerSettings.Error = (_, args) =>
                    {
                        Console.WriteLine(args.ErrorContext.Error);
                    };

                    opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                }
                );
            var settings = Configuration.Get<SmartTaskSettings>();
            services.Configure<SmartTaskSettings>(Configuration);
            // Add framework services.
            services.AddMvc(options =>
            {
                options.Filters.Add(typeof(HttpGlobalExceptionFilter));
            })
                .AddControllersAsServices();

            services.AddEntityFrameworkSqlServer()
               .AddDbContext<SmartTaskDbContext>(options =>
               {
                   options.UseMySql(settings.DefaultConnection,
                        sqlOptions =>
                        {
                            sqlOptions.MigrationsAssembly("SmartTask.Infrastructure.Migrations");
                            sqlOptions.EnableRetryOnFailure(10, TimeSpan.FromSeconds(3), new List<int>());
                        });
               }
               );

            ConfigureSwagger(services);

            ConfigureJwtAuthentication(services, settings);


            // Add application services.
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSignalR();
            services.AddOptions();

            //configure Autofac
            var container = new ContainerBuilder();
            container.Populate(services);

            container.RegisterModule(new MediatorModule());
            container.RegisterModule(new ApplicationModule());

            return new AutofacServiceProvider(container.Build());
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            var builder = new ConfigurationBuilder()
              .SetBasePath(env.ContentRootPath)
              .AddJsonFile("appsettings.json", true, true)
              .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true)
              .AddEnvironmentVariables();
            Configuration = builder.Build();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();
            app.UseMiddleware<AuthorizationErrorMiddleware>();

            app.UseSwagger()
              .UseSwaggerUI(c =>
              {
                  c.SwaggerEndpoint("/swagger/v1/swagger.json", "SmartTask.API V1");

                  c.DocumentTitle = "SmartTask API";
                  c.DocExpansion(DocExpansion.List);
              });

            app.UseStaticFiles();

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors("CorsPolicy");

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }

        #region HelperMethods


        private void ConfigureSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "SmartTask API",
                    Version = "v1",
                    Description = "The SmartTask CRM Service API"
                });

                options.OperationFilter<AuthorizeCheckOperationFilter>();

                options.AddSecurityDefinition("bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: bearer {token}\"",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    In = ParameterLocation.Header,
                    BearerFormat = "JWT",
                    Scheme = "bearer"
                });
            });
        }

        private void ConfigureJwtAuthentication(IServiceCollection services, SmartTaskSettings settings)
        {
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear(); // => remove default claims
            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(cfg =>
                {
                    cfg.RequireHttpsMetadata = false;
                    cfg.SaveToken = true;
                    cfg.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = settings.JwtIssuer,
                        ValidateAudience = true,
                        ValidAudience = settings.JwtIssuer,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.JwtKey)),
                        ClockSkew = TimeSpan.FromSeconds(15),
                        ValidateLifetime = true
                    };
                });
        }
        #endregion
    }
}
