using SmartTask.SharedKernel.Domain.Seedwork;

namespace SmartTask.Domain.AggregatesModel.PermissionAggregate
{
    public class PermissionCode : Enumeration
    {
       
        public static readonly PermissionCode Employee_Add = new PermissionCode(1, PermissionName.Employee_Add);
        public static readonly PermissionCode Employee_Cancel = new PermissionCode(2, PermissionName.Employee_Cancel);
        public static readonly PermissionCode Employee_View = new PermissionCode(3, PermissionName.Employee_View);
       

        public PermissionCode(int id, string name) : base(id, name)
        {
        }

        public PermissionCode()
        {
        }
    }
}
