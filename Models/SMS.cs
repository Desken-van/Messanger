using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Messanger.Models
{
    public class SMS
    {
        public int Id { get; set; }
        public int Sender { get; set; }
        public string Sms { get; set; }
        public int Recipient { get; set; }
        public int Number { get; set; }
    }
}
