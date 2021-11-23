using Microsoft.EntityFrameworkCore;
using SmartTask.Infrastructure.Database;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using System.Threading.Tasks;
using SmartTask.Shared.DTO.RawSql;

namespace SmartTask.Shared.Queries
{
    public class RawSqlQueries : IRawSqlQueries
    {
        private const string ROW_KEY_COLUMN = "RowKey";

        private readonly SmartTaskDbContext _context;

        public RawSqlQueries(SmartTaskDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<TableDto> ReadFromCommand(string commandText)
        {
            using (var command = _context.Database.GetDbConnection().CreateCommand())
            {
                _context.Database.OpenConnection();
#pragma warning disable CA2100 // Review SQL queries for security vulnerabilities
                command.CommandText = commandText;
#pragma warning restore CA2100 // Review SQL queries for security vulnerabilities

                using (var reader = command.ExecuteReader())
                {
                    var tableDto = await ReadResultAsync(reader);
                    return tableDto;
                }
            }
        }

        public async Task<TableDto> ReadResultAsync(DbDataReader reader)
        {
            var tableDto = new TableDto();

            for (var i = 0; i < reader.FieldCount; i++)
            {
                tableDto.Columns.Add(reader.GetName(i));
            }

            while (await reader.ReadAsync())
            {
                var rowDto = await ReadRowAsync(reader, tableDto.Columns);
                tableDto.Rows.Add(rowDto);
            }

            return tableDto;
        }

        private async Task<RowDto> ReadRowAsync(DbDataReader reader, IEnumerable<string> columns)
        {
            var rowDto = new RowDto();

            foreach (var column in columns)
            {
                var cellIndex = reader.GetOrdinal(column);

                if (column == ROW_KEY_COLUMN)
                {
                    rowDto.Key = reader.GetString(cellIndex);
                }

                var cellDto = await ReadCellAsync(reader, column);
                rowDto.Cells.Add(cellDto);
            }

            return rowDto;
        }

        private async Task<CellDto> ReadCellAsync(DbDataReader reader, string column)
        {
            var index = reader.GetOrdinal(column);
            var type = reader.GetFieldType(index);

            if (await reader.IsDBNullAsync(index))
            {
                return new CellDto(column, null, type);
            }

            return new CellDto(column, reader.GetValue(index), type);
        }
    }
}
