using System;
using System.Collections.Generic;
using System.Text;

namespace SmartTask.Infrastructure.Constants
{
   public class LoadMoreDTO
    {
        public int? Skip { get; set; }
        public int? Take { get; set; }
        public string SortField { get; set; }
        public bool OrderBy { get; set; }
    }
}
