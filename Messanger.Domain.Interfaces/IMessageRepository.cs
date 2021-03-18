using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Messanger.Domain.Core;
namespace Messanger.Domain.Interfaces
{
    public interface IMessageRepository:IDisposable
    {
        Task<IQueryable<MessageEntity>> GetMessageList();
        Task<MessageEntity> CheckMessage(string sender);
        Task<MessageEntity> GetMessage(string sender, string recepient);
        Task Create(MessageEntity item);
        Task Update(MessageEntity item);
        Task Delete(int id);
    }
}
