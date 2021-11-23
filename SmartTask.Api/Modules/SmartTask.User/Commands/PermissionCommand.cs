using System;
using System.Collections.Generic;
using System.Text;

namespace SmartTask.User.Commands
{
   public class PermissionCommand
    {
        public int PermissionId { get; set; }
        public int ScopeId { get; set; }
        public string Value { get; set; }
    }
}
