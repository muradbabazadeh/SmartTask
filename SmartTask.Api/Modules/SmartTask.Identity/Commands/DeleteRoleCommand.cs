using MediatR;

namespace SmartTask.Identity.Commands
{
    public class DeleteRoleCommand : IRequest<bool>
    {
        public int? Id { get; set; }
    }
}
