using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Messanger.Models.InterfaceModels
{
    public class MessageResponse
    {
        public string Sender { get; set; }
        public string Sms { get; set; }
        public string Recipient { get; set; }
        public DateTime Time { get; set; }
    }
}
