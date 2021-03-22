using System;
using System.Collections.Generic;
using System.Text;

namespace Messanger.Infrastructure.Data
{
    public class Message
    {
        public int Id { get; set; }
        public string Sender { get; set; }
        public string Sms { get; set; }
        public string Recipient { get; set; }
        public DateTime Time { get; set; }
 
    }
}
