using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Messanger.Domain.Core
{
    public class AccountEntity
    {         
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public string Status { get; set; }
    }
}