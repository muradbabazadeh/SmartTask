using SmartTask.Domain.AggregatesModel.PermissionAggregate;
using SmartTask.SharedKernel.Domain.Seedwork;

namespace SmartTask.Domain.AggregatesModel.RoleAggregate
{
    public class RolePermissionParameterValue : Entity
    {
        public int PermissionParameterId { get; private set; }

        public int RolePermissionId { get; private set; }

        public string Value { get; private set; }

        public PermissionParameter PermissionParameter { get; private set; }
        public RolePermission RolePermission { get; private set; }

        public RolePermissionParameterValue()
        {
        }

        public RolePermissionParameterValue(int permissionParameterId, int rolePermissionId, string value) : this()
        {
            PermissionParameterId = permissionParameterId;
            RolePermissionId = rolePermissionId;
            Value = value;
        }

        public void AddToInfo(int permissionParameterId,int rolePermissionId,string value)
        {
            PermissionParameterId = permissionParameterId;
            RolePermissionId = rolePermissionId;
            Value = value;
        }
    }
}
