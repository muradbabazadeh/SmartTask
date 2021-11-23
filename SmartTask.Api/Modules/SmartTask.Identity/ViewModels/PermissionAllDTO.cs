using System;
using System.Collections.Generic;
using System.Text;

namespace SmartTask.Identity.ViewModels
{
   public class PermissionAllDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<PermissionParametrAllDTO> Parameters { get; set; }
    }
}
