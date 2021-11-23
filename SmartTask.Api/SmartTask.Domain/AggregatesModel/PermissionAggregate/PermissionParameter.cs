using SmartTask.SharedKernel.Domain.Seedwork;
namespace SmartTask.Domain.AggregatesModel.PermissionAggregate
{
    public class PermissionParameter 
    {
        public int Id { get; set; }
        public string Name { get; private set; }

        public string Description { get; private set; }

        public string Type { get; private set; }

        public string DefaultValue { get; private set; }

        public int PermissionId { get; private set; }
        public Permission Permission { get; set; }

        public void SetDetails(PermissionParameterCode code, string description, string type, string defaultValue)
        {
            Id = code.Id;
            Name = code.Name;
            Description = description;
            Type = type;
            DefaultValue = defaultValue;
        }
    }
}
