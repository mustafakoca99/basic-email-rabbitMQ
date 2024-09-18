using System;
using System.Collections.Generic;
using System.Text;

namespace RabbitMQ.Shared.Dto
{
    public class EmailSendRequestDto
    {
        public string ToMailAddress { get; set; }
        public string CcMailAddress { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}
