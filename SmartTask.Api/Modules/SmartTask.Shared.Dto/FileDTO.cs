using System;
using System.IO;

namespace SmartTask.Shared.DTO
{
    public class FileDTO
    {
        public string Name { get; set; }

        public string Extension { get; set; }

        public long Size { get; set; }

        public Stream Content { get; set; }

        public string ContentType { get; set; }

        public DateTime? Created { get; set; }

        public DateTime? LastModified { get; set; }

        public string CreatedByName { get; set; }

        public string LastUpdatedByName { get; set; }
    }
}
