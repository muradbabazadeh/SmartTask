using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartTask.User.Commands
{
    public class UpdateUserPasswordCommand : IRequest<bool>
    {
        public int Id { get; set; }
        public string Password { get; set; }
    }
}
