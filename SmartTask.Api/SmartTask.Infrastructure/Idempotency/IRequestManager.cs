using System;
using System.Threading.Tasks;

namespace SmartTask.Infrastructure.Idempotency
{
    public interface IRequestManager
    {
        Task<bool> ExistAsync(Guid key);

        Task CreateRequestForCommandAsync<T>(Guid key);
    }
}
