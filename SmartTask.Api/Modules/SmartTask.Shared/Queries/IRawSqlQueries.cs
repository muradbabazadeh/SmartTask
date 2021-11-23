using SmartTask.Shared.DTO.RawSql;
using System.Threading.Tasks;
namespace SmartTask.Shared.Queries
{
    public interface IRawSqlQueries
    {
        Task<TableDto> ReadFromCommand(string commandText);
    }
}
