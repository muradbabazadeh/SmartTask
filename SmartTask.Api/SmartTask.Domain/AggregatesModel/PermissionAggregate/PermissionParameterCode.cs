using SmartTask.SharedKernel.Domain.Seedwork;

namespace SmartTask.Domain.AggregatesModel.PermissionAggregate
{
    public class PermissionParameterCode : Enumeration
    {

        public static PermissionParameterCode EmployeeViewAllScope = new PermissionParameterCode(1, PermissionParameterName.Scope);

        public static PermissionParameterCode EmployeeAddAllScope = new PermissionParameterCode(2, PermissionParameterName.Scope);

        public static PermissionParameterCode EmployeeCancelAllScope = new PermissionParameterCode(3, PermissionParameterName.Scope);
        public PermissionParameterCode(int id, string name) : base(id, name)
        {
        }

        public PermissionParameterCode() { }
    }
}
