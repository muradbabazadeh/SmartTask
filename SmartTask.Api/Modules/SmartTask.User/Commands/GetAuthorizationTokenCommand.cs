using MediatR;
using SmartTask.User.Commands.Models;
namespace SmartTask.User.Commands
{
    public class GetAuthorizationTokenCommand : IRequest<JwtTokenDTO>
    {
        public string UserName { get; set; }

        public string Password { get; set; }
    }
}
