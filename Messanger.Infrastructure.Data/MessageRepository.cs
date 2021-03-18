using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Messanger.Domain.Core;
using Messanger.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Messanger.Infrastructure.Data
{
    public class MessageRepository : IMessageRepository
    {
        private UsersContext db;
        public MessageRepository(UsersContext context)
        {
            db = context;
        }

        public async Task<IQueryable<MessageEntity>> GetMessageList()
        {
            IQueryable<MessageEntity> sms = db.Sms;
            await db.SaveChangesAsync();
            return sms;
        }

        public async Task<MessageEntity> CheckMessage(string sender)
        {
            MessageEntity sms = await db.Sms.FirstOrDefaultAsync(x => x.Sender == sender);
            return sms;
        }
        public async Task<MessageEntity> GetMessage(string sender,string recepient)
        {
            MessageEntity sms = await db.Sms.FirstOrDefaultAsync(x => x.Sender == sender && x.Recipient == recepient);
            return sms;
        }
        public async Task Create(MessageEntity sms)
        {
           await db.Sms.AddAsync(sms);
           await db.SaveChangesAsync();
        }

        public async Task Update(MessageEntity sms)
        {
            await Task.Run(() =>db.Entry(sms).State = EntityState.Modified);
            await db.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            MessageEntity sms = await db.Sms.FindAsync(id);
            if (sms != null)
                await Task.Run(() => db.Sms.Remove(sms));
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
