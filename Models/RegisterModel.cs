using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Messanger.Models
{
    public class RegisterModel
    {
        public string Login { get; set; }
        public string Password { get; set; }
    }
}
