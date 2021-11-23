using System.Collections.Generic;
namespace SmartTask.Core.Api.Infrastructure.DBSeed
{
    public class PermissionSeedDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public List<ParameterSeedDTO> Parameters { get; set; }
    }
}
