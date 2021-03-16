using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Messanger.Domain.Core;

namespace Messanger.Services.Interfaces
{
    public interface IAddSms
    {
        MessageEntity AddSms(AccountEntity sender,string text,AccountEntity recepient);              
    }
}
