using MediatR;

namespace SmartTask.User.Commands
{
    public class ForgotPasswordCommand : IRequest<bool>
    {
        public string UserName { get; set; }
    }
}
