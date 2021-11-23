using System;
using System.Collections.Generic;
using System.Text;

namespace SmartTask.Identity.ViewModels.FilterModel
{
   public class UserFilterDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int? RoleId { get; set; }
    }
}
