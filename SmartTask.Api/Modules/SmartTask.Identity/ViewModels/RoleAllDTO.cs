using System;
using System.Collections.Generic;
using System.Text;

namespace SmartTask.Identity.ViewModels
{
   public class RoleAllDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<RolePermissionDTO> Permissions { get; set; }
    }
}
