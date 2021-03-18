using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Messanger.Domain.Core;
namespace Messanger.Domain.Interfaces
{
    public interface IMessageRepository:IDisposable
    {
        Task<MessageEntity[]> GetMessageList();
        Task<MessageEntity> CheckMessage(int sender);
        Task<MessageEntity> GetMessage(int sender, int recepient);
        Task Create(MessageEntity item);
        Task Update(MessageEntity item);
        Task Delete(int id);
    }
}
