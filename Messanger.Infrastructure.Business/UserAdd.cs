using Microsoft.Analytics.Interfaces;
using Microsoft.Analytics.Interfaces.Streaming;
using Microsoft.Analytics.Types.Sql;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Messanger.Domain.Core;
using Messanger.Services.Interfaces;

namespace Messanger.Infrastructure.Business
{
    public class UserAdd :IAddUser
    {
        public AccountEntity AddUser(string username,string password)
        {
            AccountEntity user = new AccountEntity
            {
                Login = username,
                Password = password,
                Role = "User",
                Status = "Active"
            };
            return user;
        }
    }
}