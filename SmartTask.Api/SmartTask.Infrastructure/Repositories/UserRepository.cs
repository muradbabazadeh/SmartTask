using Microsoft.EntityFrameworkCore;
using SmartTask.Domain.AggregatesModel.UserAggregate;
using SmartTask.Infrastructure.Database;
using SmartTask.SharedKernel.Infrastructure;

namespace SmartTask.Infrastructure.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public sealed override DbContext Context { get; protected set; }

        public UserRepository(SmartTaskDbContext context)
        {
            Context = context;
        }
    }
}
