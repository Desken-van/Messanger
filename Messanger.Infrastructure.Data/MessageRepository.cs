using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Messanger.Domain.Core;
using Messanger.Domain.Interfaces;


namespace Messanger.Infrastructure.Data
{
    public class MessageRepository : IMessageRepository
    {
        private UsersContext db;
        public MessageRepository(UsersContext context)
        {
            db = context;
        }

        public IEnumerable<MessageEntity> GetAccountList()
        {
            return db.Sms.ToArray();
        }

        public MessageEntity CheckMessage(int sender)
        {
            MessageEntity sms = db.Sms.FirstOrDefault(x => x.Sender == sender);
            return sms;
        }
        public MessageEntity GetMessage(int sender,int recepient)
        {
            MessageEntity sms = db.Sms.FirstOrDefault(x => x.Sender == sender && x.Recipient == recepient);
            return sms;
        }
        public void Create(MessageEntity item)
        {
            db.Sms.Add(item);
        }

        public void Update(MessageEntity item)
        {
            db.Entry(item).State = (Microsoft.EntityFrameworkCore.EntityState)EntityState.Modified;
        }

        public void Delete(int id)
        {
            MessageEntity sms = db.Sms.Find(id);
            if (sms != null)
                db.Sms.Remove(sms);
        }

        public void Save()
        {
            db.SaveChanges();
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
