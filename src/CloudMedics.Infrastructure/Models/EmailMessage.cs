using System;
using System.Collections.Generic;
using System.Net.Mail;
namespace CloudMedics.Infrastructure.Models
{
    public class EmailMessage
    {
        public string Sender { get; set; }
        public string Recipient { get; set; }
        public string FromTitle { get; set; }
        public string To { get; set; }
        public List<string> CC { get; set; }
        public List<string> BCC { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public bool IsHtml { get; set; }
        public bool HasAttachment { get; set; }
        public List<Attachment> Attachments { get; set; }

        public override string ToString()
        {
            return $"{Subject} : From : {Sender} -Recipient :{Recipient}";

        }
    }
}
