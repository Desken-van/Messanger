using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Messanger.Domain.Core;
using Messanger.Services.Interfaces;
using Messanger.Infrastructure.Data;

namespace Messanger.Infrastructure.Business
{
    public class SmsAdd : IAddSms
    {
        UsersContext db;
        public SmsAdd(UsersContext context)
        {
            db = context;
        }
        public MessageEntity AddSms(AccountEntity sender, string text, AccountEntity recepient)
        {          
            DateTime time = DateTime.Now;
            MessageEntity sms = new MessageEntity
            {
                Sender = sender.Login,
                Sms = text,
                Recipient = recepient.Login,
                Time = time
            };
            return sms;
        }
    }
}