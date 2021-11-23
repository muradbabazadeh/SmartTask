using System.Collections.Generic;

namespace SmartTask.Shared.DTO.RawSql
{
    public class RowDto
    {
        public string Key { get; set; }

        public RowState State { get; set; }

        public List<CellDto> Cells { get; set; }

        public RowDto()
        {
            State = RowState.Default;
            Cells = new List<CellDto>();
        }
    }
}
