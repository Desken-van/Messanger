using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Messanger.Domain.DataBase;
using Messanger.Domain.Interfaces;
using Messanger.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Messanger.Domain.Repository
{
    public class MessageRepository : IMessageRepository
    {
        private UsersContext db;
        public MessageRepository(UsersContext context)
        {
            db = context;
        }

        public async Task<IEnumerable<Message>> GetMessageList(string sender,string recepient)
        {
            MessageEntity[] array =await db.Sms.ToArrayAsync();
            var  data = from p in array
                            where (p.Sender == sender && p.Recipient == recepient) || (p.Recipient == sender && p.Sender == recepient)
                            orderby p.Time
                             select p;
            var datasms = data.Select(x => new Message { 
                Id = x.Id,                
                Sender = x.Sender,
                Sms = x.Sms,
                Recipient = x.Recipient,
                Time = x.Time
            });
            return datasms;
        }
        public async Task<List<string>> GetMessageSite(string sender,string recepient)
        {
            MessageEntity[] array = await db.Sms.ToArrayAsync();
            IEnumerable<string> data = from p in array
                                       where (p.Sender == sender && p.Recipient == recepient) || (p.Recipient == sender && p.Sender == recepient)
                                       orderby p.Time
                                       select $"____________________________________\n||From:{p.Sender}|\n \n {p.Sms} \n \n|To:{p.Recipient}|  |{p.Time}||\n____________________________________\n";
            
            List<string> sms = new List<string>();
            foreach (string s in data)
            {
                sms.Add(s);
            }       
            return sms;
        } 
        public async Task Add(string sender, string text, string recepient)
        {
            DateTime time = DateTime.Now;
            MessageEntity sms = new MessageEntity
            {
                Sender = sender,
                Sms = text,
                Recipient = recepient,
                Time = time
            };
            await Create(sms);       
        }
        public async Task Create(MessageEntity sms)
        {
           await db.Sms.AddAsync(sms);
           await db.SaveChangesAsync();
        }

        public async Task Update(Message sms)
        {
            var original = await db.Sms.FindAsync(sms.Id);
            db.Entry(original).CurrentValues.SetValues(sms);
            await db.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            MessageEntity sms = await db.Sms.FindAsync(id);
            if (sms != null)
                       db.Sms.Remove(sms);
            await db.SaveChangesAsync();
        }

        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    db.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
