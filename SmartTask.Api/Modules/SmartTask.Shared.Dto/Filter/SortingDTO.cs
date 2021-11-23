using System;
using System.Collections.Generic;
using System.Text;

namespace SmartTask.Shared.DTO.Filter
{
    public class SortingDTO
    {
        public string Field { get; set; } = "recordDateTime";

        public string Direction { get; set; } = "descend";
    }
}
