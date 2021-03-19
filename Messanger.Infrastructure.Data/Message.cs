using Messanger.Domain.DataBase;
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

        public static implicit operator Message(MessageEntity messageentity)
        {
            Message message = new Message();
            message.Id = messageentity.Id;
            message.Sender = messageentity.Sender;
            message.Sms = messageentity.Sms;
            message.Recipient = messageentity.Recipient;
            message.Time = messageentity.Time;
            return message;
        }
    }
}
