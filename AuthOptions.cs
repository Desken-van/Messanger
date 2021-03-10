using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Messanger
{
    public class AuthOptions
    {
        public const string Option = "Option";
        public string ISSUER { get; set; } // издатель токена
        public string AUDIENCE { get; set; } // потребитель токена       
        public string KEY { get; set; }   // ключ для шифрации
        
        public const int LIFETIME = 1;

        public static SymmetricSecurityKey GetSymmetricSecurityKey(string key)
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key));
        }
    }
}
