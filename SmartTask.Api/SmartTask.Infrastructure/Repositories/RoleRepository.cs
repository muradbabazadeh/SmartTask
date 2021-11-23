using Microsoft.EntityFrameworkCore;
using SmartTask.Domain.AggregatesModel.RoleAggregate;
using SmartTask.Infrastructure.Database;
using SmartTask.SharedKernel.Infrastructure;

namespace SmartTask.Infrastructure.Repositories
{
    public class RoleRepository : Repository<Role>, IRoleRepository
    {
        public sealed override DbContext Context { get; protected set; }

        public RoleRepository(SmartTaskDbContext context)
        {
            Context = context;
        }
    }
}
