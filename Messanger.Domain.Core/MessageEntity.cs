using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messanger.Domain.Core
{
    public class MessageEntity
    {
        public int Id { get; set; }
        public int Sender { get; set; }
        public string Sms { get; set; }
        public int Recipient { get; set; }
        public int Number { get; set; }
    }
}
