using SmartTask.Domain.AggregatesModel.PermissionAggregate;
using SmartTask.SharedKernel.Domain.Seedwork;
using System.Collections.Generic;
namespace SmartTask.Domain.AggregatesModel.UserAggregate
{
    public class UserPermission : Entity
    {
        public int UserId { get; private set; }

        public int PermissionId { get; private set; }

        public Permission Permission { get; private set; }

        private readonly List<UserPermissionParameterValue> _parameterValues;
        public IReadOnlyCollection<UserPermissionParameterValue> ParameterValues => _parameterValues;

        public UserPermission()
        {
            _parameterValues = new List<UserPermissionParameterValue>();
        }

        public UserPermission(int permissionId) : this()
        {
            PermissionId = permissionId;
        }
        public UserPermission(int userId, int permissionId) : this(permissionId)
        {
            UserId = userId;
            PermissionId = permissionId;
        }

        public void AddToInfo(int userId,int permissionId)
        {
            UserId = userId;
            PermissionId = permissionId;
        }
    }
}
