using System;
using System.Collections.Generic;
using System.Text;

namespace SmartTask.Identity.ViewModels
{
   public class UserAllProfileDto
    {
        public int TotalCount { get; set; }
        public IEnumerable<UserProfileDTO> Data { get; set; }
    }
}
