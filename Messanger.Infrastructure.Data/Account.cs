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
    }
}
