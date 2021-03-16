using Microsoft.Analytics.Interfaces;
using Microsoft.Analytics.Interfaces.Streaming;
using Microsoft.Analytics.Types.Sql;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Messanger.Domain.Core;

namespace Messanger.Domain.Interfaces
{
    public interface IAccountRepository : IDisposable
    {
        IEnumerable<AccountEntity> GetAccountList();
        AccountEntity CheckAccount(string username);
        AccountEntity GetAccount(string username,string password);
        void Create(AccountEntity item);
        void Update(AccountEntity item);
        void Delete(int id);
        void Save();
    }
}