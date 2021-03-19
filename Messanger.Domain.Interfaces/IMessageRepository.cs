using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Messanger.Domain.DataBase;
using Messanger.Infrastructure.Data;

namespace Messanger.Domain.Interfaces
{
    public interface IMessageRepository:IDisposable
    {
        Task<List<Message>> GetMessageList(string sender,string recepient);
        Task<List<string>> GetMessageSite(string sender, string recepient);
        Task Add(string sender, string text, string recepient);
        Task Create(MessageEntity item);
        Task Update(Message item);
        Task Delete(int id);
    }
}
