using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Messanger.Domain.Core;
using System.Threading.Tasks;

namespace Messanger.Domain.Interfaces
{
    public interface IAccountRepository : IDisposable
    {
        Task<AccountEntity[]> GetAccountList();
        Task<AccountEntity> CheckAccount(string username);
        Task<AccountEntity> GetAccount(string username,string password);
        Task Create(AccountEntity item);
        Task Update(AccountEntity item);
        Task Delete(int id);
    }
}