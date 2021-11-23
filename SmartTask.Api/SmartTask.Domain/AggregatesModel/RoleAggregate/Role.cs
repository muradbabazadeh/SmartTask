using SmartTask.SharedKernel.Domain.Seedwork;
using System.Collections.Generic;

namespace SmartTask.Domain.AggregatesModel.RoleAggregate
{
    public class Role : Entity, IAggregateRoot
    {
        public string Name { get; private set; }

        public string Description { get; private set; }

        private readonly List<RolePermission> _permissions;
        public IReadOnlyCollection<RolePermission> Permissions => _permissions;

        protected Role()
        {
            _permissions = new List<RolePermission>();
        }

        public Role(string name) : this()
        {
            Name = name;
        }

        public RolePermission AddPermission(int? permissionId)
        {
            var rolePermission = new RolePermission(permissionId);
            _permissions.Add(rolePermission);
            return rolePermission;
        }

        public void SetDetails(string name)
        {
            Name = name;
        }
    }
}
