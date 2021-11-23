using MediatR;
using SmartTask.UserDetails.Commands;
using SmartTask.User.Commands.Models;

namespace SmartTask.UserDetails.Commands
{
   public class RefreshUserTokenCommand : IRequest<JwtTokenDTO>
    {
        public string RefreshToken { get; set; }
    }
}
