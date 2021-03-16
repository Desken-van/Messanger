using System;
using System.Collections.Generic;

using Messanger.Domain.Core;
namespace Messanger.Domain.Interfaces
{
    public interface IMessageRepository:IDisposable
    {
        IEnumerable<MessageEntity> GetAccountList();
        MessageEntity CheckMessage(int sender);
        MessageEntity GetMessage(int sender, int recepient);
        void Create(MessageEntity item);
        void Update(MessageEntity item);
        void Delete(int id);
        void Save();
    }
}
