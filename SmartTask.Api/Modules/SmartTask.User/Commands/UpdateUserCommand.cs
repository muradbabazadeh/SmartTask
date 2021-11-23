using MediatR;
using System;
namespace SmartTask.User.Commands
{
    public class UpdateUserCommand : IRequest<bool>
    {
        public int? Id { get; set; }


        public string Email { get; set; }


        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int RoleId { get; set; }
    }
}
