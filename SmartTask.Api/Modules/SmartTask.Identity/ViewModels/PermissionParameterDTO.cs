using System.Collections.Generic;
namespace SmartTask.Identity.ViewModels
{
    public class PermissionParameterDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string DefaultValue { get; set; }

        public List<string> Values { get; set; }
    }
}
