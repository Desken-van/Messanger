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
    public class AccountRepository : IAccountRepository
    {
        private UsersContext db;
        public AccountRepository(UsersContext context)
        {
            db = context;
        }

        public async Task<AccountEntity[]> GetAccountList()
        {
            AccountEntity[] user = await db.Logins.ToArrayAsync();
            return user;
        }

        public async Task<AccountEntity> CheckAccount(string username)
        {
            AccountEntity user = await db.Logins.FirstOrDefaultAsync(x => x.Login == username);
            return user;
        }
        public async Task<AccountEntity> GetAccount(string username,string password)
        {
            AccountEntity user = await db.Logins.FirstOrDefaultAsync(x => x.Login == username && x.Password == password);
            return user;
        }
        public async Task Create(AccountEntity account)
        {
           await db.Logins.AddAsync(account);
           await db.SaveChangesAsync();
        }

        public async Task Update(AccountEntity account)
        {
            await Task.Run(() => db.Entry(account).State = EntityState.Modified);
            await  db.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            AccountEntity account = await db.Logins.FindAsync(id);
            if (account != null)
                await Task.Run(() => db.Logins.Remove(account));
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
