using MediatR;
using System.Collections.Generic;

namespace SmartTask.Identity.Commands
{
    public class UpdateRoleCommand : IRequest<bool>
    {
        public int? Id { get; set; }
        public string Name { get; set; }

        public List<PermissionDTOForCreateRole> Permissons { get; set; }
    }
}
