using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartTask.User.Commands
{
    public class CheckTokenVerifyInputCommand : IRequest<bool>
    {
        public string Token { get; set; }

        public string UserName { get; set; }
    }
}
