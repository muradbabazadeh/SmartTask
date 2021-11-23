using SmartTask.Domain.AggregatesModel.PermissionAggregate;
using SmartTask.SharedKernel.Domain.Seedwork;
using System.Collections.Generic;
namespace SmartTask.Domain.AggregatesModel.RoleAggregate
{
    public class RolePermission : Entity
    {
        public int? RoleId { get; private set; }

        public int? PermissionId { get; private set; }

        public Permission Permission { get; private set; }

        private readonly List<RolePermissionParameterValue> _parameterValues;
        public IReadOnlyCollection<RolePermissionParameterValue> ParameterValues => _parameterValues;

        public RolePermission()
        {
            _parameterValues = new List<RolePermissionParameterValue>();
        }

        public RolePermission(int? permissionId) : this()
        {
            PermissionId = permissionId;
        }
    }
}
