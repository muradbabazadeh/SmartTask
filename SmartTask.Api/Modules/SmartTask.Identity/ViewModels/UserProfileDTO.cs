using System;
using System.Collections.Generic;
namespace SmartTask.Identity.ViewModels
{
    public class UserProfileDTO
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime Birthday { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public int? RegionId { get; set; }

        public int? RoleId { get; set; }
        public string RoleName { get; set; }
        public string RegionName { get; set; }
        public int? DepartmentId { get; set; }
        public string DepartmentName { get; private set; }
        public bool Locked { get; private set; }

        public IDictionary<string, PermissionDTO> Permissions { get; set; }
    }
}
