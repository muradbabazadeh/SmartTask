using MediatR;
using System.Collections.Generic;

namespace SmartTask.Identity.Commands
{
    public class CreateRoleCommand : IRequest<bool>
    {
        public string Name { get; set; }
        public List<PermissionDTOForCreateRole> Permissons { get; set; }
    }
}
