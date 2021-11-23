using System;
using System.Collections.Generic;
using System.Text;

namespace SmartTask.Identity.Commands
{
   public class PermissionDTOForCreateRole
    {

        public int Id { get; set; }
        public int? ScopeId { get; set; }
        public string? Value { get; set; }
    }
}
