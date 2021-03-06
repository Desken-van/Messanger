﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Messanger.Models.ProgramModels
{
    public class AuthorizationOptions
    {
        public string Issuer { get; set; } 
        public string Audience { get; set; }       
        public string Key { get; set; }   
        public int Lifetime { get; set; }        
    }
}
