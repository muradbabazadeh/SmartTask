using SmartTask.Domain.AggregatesModel.RoleAggregate;
using SmartTask.SharedKernel.Domain.Seedwork;

namespace SmartTask.Domain.AggregatesModel.UserAggregate
{
    public class UserRole : Entity
    {
        public int UserId { get; private set; }

        public int? RoleId { get; private set; }

        public Role Role { get; private set; }

        public UserRole()
        {
        }

        public UserRole(int? roleId) : this()
        {
            RoleId = roleId;
        }

        public UserRole(int userId, int? roleId) : this(roleId)
        {
            UserId = userId;
            RoleId = roleId;
        }

        public void AddToInfo(int userId,int roleId)
        {
            UserId = userId;
            RoleId = roleId;
        }
    }
}
