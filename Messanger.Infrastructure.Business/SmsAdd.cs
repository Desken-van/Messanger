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
            MessageEntity counter = db.Sms.OrderByDescending(x => x.Number).FirstOrDefault();
            int count = 1;
            if (counter.Number != 0)
            {
                count = 1 + counter.Number;
            }
            MessageEntity sms = new MessageEntity
            {
                Sender = sender.Id,
                Sms = text,
                Recipient = recepient.Id,
                Number = count
            };
            return sms;
        }
    }
}