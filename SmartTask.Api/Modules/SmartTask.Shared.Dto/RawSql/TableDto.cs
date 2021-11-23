using System;
using System.Collections.Generic;
using System.Text;

namespace SmartTask.Shared.DTO.RawSql
{
    public class TableDto
    {
        public List<string> Columns { get; set; }

        public List<RowDto> Rows { get; set; }

        public TableDto()
        {
            Columns = new List<string>();
            Rows = new List<RowDto>();
        }
    }
}
