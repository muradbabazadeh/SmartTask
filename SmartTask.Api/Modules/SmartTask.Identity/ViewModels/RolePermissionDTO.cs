using System;
using System.Collections.Generic;
using System.Text;

namespace SmartTask.Identity.ViewModels
{
   public class RolePermissionDTO
    {
        public int RoleId { get; set; }
        public int PermissionId { get; set; }
        public string PermissionName { get; set; }
        public int ScopeId { get; set; }
        public string Value { get; set; }

    }
}
