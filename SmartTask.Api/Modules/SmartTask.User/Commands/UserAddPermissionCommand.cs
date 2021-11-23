using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartTask.User.Commands
{
   public class UserAddPermissionCommand : IRequest<int>
    {
        public int UserId { get; set; }
        public List<PermissionCommand> Permissions { get; set; }
    }
}
