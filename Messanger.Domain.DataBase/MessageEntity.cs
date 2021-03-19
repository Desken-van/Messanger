using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messanger.Domain.DataBase
{
    public class MessageEntity
    {
        public int Id { get; set; }
        public string Sender { get; set; }
        public string Sms { get; set; }
        public string Recipient { get; set; }
        public DateTime Time { get; set; }
    }
}
