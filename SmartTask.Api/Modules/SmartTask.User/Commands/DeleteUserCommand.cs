using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartTask.UserDetails.Commands
{
   public class DeleteUserCommand : IRequest<bool>
    {
        public List<int> Id { get; set; }
    }
}
