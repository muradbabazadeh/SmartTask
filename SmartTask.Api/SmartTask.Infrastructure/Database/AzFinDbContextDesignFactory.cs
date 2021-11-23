using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SmartTask.Infrastructure.Database
{
   public class SmartTaskDbContextDesignFactory : IDesignTimeDbContextFactory<SmartTaskDbContext>
    {
        public SmartTaskDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<SmartTaskDbContext>()

                //configure your server information
                .UseMySql("server =.; port = 3306; database = smart_task; uid = crm; password = ?? ", sqlOptions =>
                {
                    sqlOptions.MigrationsAssembly("SmartTask.Infrastructure.Migrations");
                });

            return new SmartTaskDbContext(optionsBuilder.Options, new NoMediator());
        }

        private class NoMediator : IMediator
        {
            public Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = default) where TNotification : INotification
            {
                return Task.CompletedTask;
            }

            public Task Publish(object notification, CancellationToken cancellationToken = default)
            {
                return Task.CompletedTask;
            }

            public Task<TResponse> Send<TResponse>(IRequest<TResponse> request,
                CancellationToken cancellationToken = default(CancellationToken))
            {
                return Task.FromResult(default(TResponse));
            }

            public Task Send(IRequest request, CancellationToken cancellationToken = default(CancellationToken))
            {
                return Task.CompletedTask;
            }

            public Task<object> Send(object request, CancellationToken cancellationToken = default)
            {
                throw new System.NotImplementedException();
            }
        }
    }
}
