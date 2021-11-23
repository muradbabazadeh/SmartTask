using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartTask.User.Commands
{
   public class ChangeUserStatusCommand : IRequest<bool>
    {
        public int Id { get; set; }
        public bool Status { get; set; }
    }
}
