using Messanger.Domain.DataBase;
using Messanger.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messanger.Domain.Interfaces
{
    public interface IAccountRepository : IDisposable
    {
        Task<List<Account>> GetAccountList();
        Task<List<string>> GetAccountSite();
        Task<Account> CheckAccount(string username);
        Task<Account> GetAccount(string username,string password);
        Task Add(string username, string password);
        Task Create(AccountEntity item);
        Task Update(Account item);
        Task Delete(int id);
    }
}