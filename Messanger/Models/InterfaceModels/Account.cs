using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Messanger.Models.InterfaceModels
{
    public class Account
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    
        public string Status { get; set; }
    }
}
