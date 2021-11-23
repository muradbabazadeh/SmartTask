using SmartTask.Domain.AggregatesModel.PermissionAggregate;
using SmartTask.SharedKernel.Domain.Seedwork;
namespace SmartTask.Domain.AggregatesModel.UserAggregate
{
    public class UserPermissionParameterValue : Entity
    {
        public int PermissionParameterId { get; private set; }
        public int UserPermissionId { get; private set; }

        public string Value { get; private set; }

        public PermissionParameter PermissionParameter { get; private set; }
        public UserPermission UserPermission { get; private set; }

        public UserPermissionParameterValue()
        {
        }

        public UserPermissionParameterValue(int permissionParameterId, string value) : this()
        {
            PermissionParameterId = permissionParameterId;
            Value = value;
        }

        public void AddToInfo(int permissionParametrId,string value,int userPermissionId)
        {
            PermissionParameterId = permissionParametrId;
            Value = value;
            UserPermissionId = userPermissionId;
        }
    }
}
