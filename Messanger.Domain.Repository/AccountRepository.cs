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
    public class AccountRepository : IAccountRepository
    {
        private UsersContext db;
        public AccountRepository(UsersContext context)
        {
            db = context;
        }
        public async Task<List<Account>> GetAccountList()
        {
            AccountEntity[] bigdata = await db.Logins.ToArrayAsync();
            var users = from p in bigdata
                        where p.Role == "User" && p.Status == "Active"
                        select p;
            var admins = from p in bigdata
                         where p.Role == "Admin" && p.Status == "Active"
                         select p;

            var blocks = from p in bigdata
                         where p.Role == "User" && p.Status == "Blocked"
                         select p;

            var users_and_admins = users.Union(admins);
            var datausers = users_and_admins.Union(blocks);
            List<Account> data = new List<Account>();
            foreach (AccountEntity s in datausers)
            {
                data.Add(s);
            }
            return data;      
        }
        public async Task<List<string>> GetAccountSite()
        {
            AccountEntity[] bigdata =await db.Logins.ToArrayAsync();
            IEnumerable<string> users = from p in bigdata
                                       where p.Role == "User" && p.Status == "Active"
                                       select p.Login;
            IEnumerable<string> admins = from p in bigdata
                                        where p.Role == "Admin" && p.Status == "Active"
                                        select "$" + p.Login;

            IEnumerable<string> blocks = from p in bigdata
                                        where p.Role == "User" && p.Status == "Blocked"
                                        select p.Login + "*";

            var users_and_admins = admins.Union(users);
            var datausers = users_and_admins.Union(blocks);
            List<string> data = new List<string>();
            foreach(string s in datausers)
            {
                data.Add(s);
            } 
            return data;
        }
        public async Task<Account> CheckAccount(string username)
        {
            AccountEntity user = await db.Logins.FirstOrDefaultAsync(x => x.Login == username);           
            return user;
        }
        public async Task<Account> GetAccount(string username,string password)
        {
            AccountEntity user = await db.Logins.FirstOrDefaultAsync(x => x.Login == username && x.Password == password);
            return user;
        }
        public async Task Add(string username, string password)
        {
            AccountEntity user = new AccountEntity
            {
                Login = username,
                Password = password,
                Role = "User",
                Status = "Active"
            };
            await Create(user);
        }
        public async Task Create(AccountEntity account)
        {           
            await db.Logins.AddAsync(account);
            await db.SaveChangesAsync();
        }

        public async Task Update(Account account)
        {
            var original = await db.Logins.FindAsync(account.Id);
            db.Entry(original).CurrentValues.SetValues(account);
            //db.Logins.Update(account);
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
