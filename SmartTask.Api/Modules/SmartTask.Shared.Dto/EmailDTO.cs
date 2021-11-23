using System;
using System.Collections.Generic;
using System.Text;

namespace SmartTask.Shared.DTO
{
    public class MailAttachmentDto
    {
        public string Uri { get; set; }

        public string Name { get; set; }
    }

    public class EmailDTO
    {
        public string Email { get; set; }

        public string Body { get; set; }

        public string Subject { get; set; }

        public List<MailAttachmentDto> Attachments { get; set; } = new List<MailAttachmentDto>();
    }
}
