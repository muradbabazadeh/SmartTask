using System;
using System.Collections.Generic;
using System.Text;

namespace SmartTask.Infrastructure.Constants
{
    public class SmsOptions
    {
        public static readonly string Sms = "Sms";

        public string Title { get; set; }
        public string Url { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
    }
}
