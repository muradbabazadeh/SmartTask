using MediatR;
using SmartTask.User.Commands.Models;

namespace SmartTask.User.Commands
{
    public class ChangePasswordCommand : IRequest<JwtTokenDTO>
    {
        public string OldPassword { get; set; }

        public string NewPassword { get; set; }

        public string NewPasswordConfirm { get; set; }
    }
}
