using Messanger.Domain.DataBase;
using System;
using System.Collections.Generic;
using System.Text;

namespace Messanger.Infrastructure.Data
{
    public class Account
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public string Status { get; set; }
        
        public static implicit operator Account(AccountEntity accountentity)
        {
            Account account = new Account();
            account.Id = accountentity.Id;
            account.Login = accountentity.Login;
            account.Password = accountentity.Password;
            account.Role = accountentity.Role;
            account.Status = accountentity.Status;
            return account;
        }
        public static implicit operator AccountEntity(Account account)
        {
            AccountEntity accountentity = new AccountEntity();
            accountentity.Id = account.Id;
            accountentity.Login = account.Login;
            accountentity.Password = account.Password;
            accountentity.Role = account.Role;
            accountentity.Status = account.Status;
            return accountentity;
        }
    }
}
