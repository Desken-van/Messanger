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
    public class AccountRepository : IAccountRepository
    {
        private UsersContext db;
        public AccountRepository(UsersContext context)
        {
            db = context;
        }

        public IEnumerable<AccountEntity> GetAccountList()
        {
            return db.Logins.ToArray();
        }

        public AccountEntity CheckAccount(string username)
        {
            AccountEntity user = db.Logins.FirstOrDefault(x => x.Login == username);
            return user;
        }
        public AccountEntity GetAccount(string username,string password)
        {
            AccountEntity user = db.Logins.FirstOrDefault(x => x.Login == username && x.Password == password);
            return user;
        }
        public void Create(AccountEntity book)
        {
            db.Logins.Add(book);
        }

        public void Update(AccountEntity book)
        {
            db.Entry(book).State = (Microsoft.EntityFrameworkCore.EntityState)EntityState.Modified;
        }

        public void Delete(int id)
        {
            AccountEntity book = db.Logins.Find(id);
            if (book != null)
                db.Logins.Remove(book);
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
