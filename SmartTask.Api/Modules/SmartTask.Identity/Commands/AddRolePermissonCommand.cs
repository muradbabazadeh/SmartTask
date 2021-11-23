using MediatR;
using System.Collections.Generic;
namespace SmartTask.Identity.Commands
{
    public class AddRolePermissonCommand : IRequest<bool>
    {
        public int? RoleId { get; set; }
        public List<int?> Permissons { get; set; }
    }
}
