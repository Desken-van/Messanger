using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Messanger.Models.ProgramModels;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using System.Text;
using Messanger.DataBase;
using Messanger.Models.InterfaceModels;
using Messanger.Domain.Core;
using Messanger.Domain.Interfaces;
using Messanger.Services.Interfaces;

namespace Messanger.Controllers
{
    public class AccountController : Controller
    {
        UsersContext db;
        IAccountRepository repoacc;
        IAddUser adduser;
        public AccountController(UsersContext context, IConfiguration configuration, IAccountRepository r, IAddUser a)
        {
            db = context;
            Configuration = configuration;
            repoacc = r;
            adduser = a;
        }
        public IConfiguration Configuration { get; }
        public Models.ProgramModels.AuthorizationOptions authoption { get; private set; }
           
        [HttpPost("/token")]
        public IActionResult Token(string username, string password)
        {
            //AccountEntity user = db.Logins.FirstOrDefault(x => x.Login == username);           
            AccountEntity user = repoacc.CheckAccount(username);
            if (user == null)
            {
                return BadRequest();
            }
            bool confirm;
            confirm = VerifyHashedPassword(user.Password,password);
            if (confirm == false)
            {
                return null;
            }
            else 
            {
                //AccountEntity account = db.Logins.FirstOrDefault(x => x.Login == username && x.Password == user.Password);
                AccountEntity account = repoacc.GetAccount(username,user.Password);
                if (account == null)
                {
                    return BadRequest(new { errorText = "Invalid username or password." });
                }
                Account person = new Account
                {
                    Login = account.Login,
                    Password = account.Password,
                    Role = account.Role,
                    Status = account.Status
                };               
                var identity = GetIdentity(person);
                authoption = Configuration.GetSection("Option").Get<Models.ProgramModels.AuthorizationOptions>();
                var response = new
                {
                    access_token = JWT(authoption,identity),
                    username = identity.Name,
                };

                return Json(response);
            }            
        }
        private static dynamic JWT(Models.ProgramModels.AuthorizationOptions option ,ClaimsIdentity identity)
        {
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(option.Key));
            var now = DateTime.UtcNow;
            var jwt = new JwtSecurityToken(
                    issuer: option.Issuer,
                    audience: option.Audience,
                    notBefore: now,
                    claims: identity.Claims,
                    expires: now.Add(TimeSpan.FromMinutes(option.Lifetime)),
                    signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
            return encodedJwt;
        }
        private ClaimsIdentity GetIdentity(Account account)
        {
            Account login = account;
            if (login != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, login.Login),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, login.Role)
                };
                ClaimsIdentity claimsIdentity =
                new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);
                return claimsIdentity;
            }
            return null;
        }
        [HttpPost("/register")]
        public IActionResult Register(string username, string password)
        {
            if ((password.Length > 12 && password.Length < 4)||username == null)
            {
                return BadRequest(new { errorText = "Invalid username or password." });
            }
            else {              
                AccountEntity[] bigdata = (AccountEntity[])repoacc.GetAccountList();
                for (int i = 0; i < bigdata.Length; i++)
                {
                    if (username == bigdata[i].Login)
                    {
                        return BadRequest();
                    }
                }
                AccountEntity user = adduser.AddUser(username, HashPassword(password));               
                if (user == null)
                {
                    return BadRequest();
                }
                repoacc.Create(user);
                repoacc.Save();
                var response = Ok();
                return Json(response);
            }
        }
        public static string HashPassword(string password)
        {
            byte[] salt;
            byte[] buffer2;
            if (password == null)
            {
                throw new ArgumentNullException("password");
            }
            using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(password, 0x10, 0x3e8))
            {
                salt = bytes.Salt;
                buffer2 = bytes.GetBytes(0x20);
            }
            byte[] dst = new byte[0x31];
            Buffer.BlockCopy(salt, 0, dst, 1, 0x10);
            Buffer.BlockCopy(buffer2, 0, dst, 0x11, 0x20);
            return Convert.ToBase64String(dst);
        }
        public static bool VerifyHashedPassword(string hashedPassword, string password)
        {
            byte[] buffer4;
            if (hashedPassword == null)
            {
                return false;
            }
            if (password == null)
            {
                throw new ArgumentNullException("password");
            }
            byte[] src = Convert.FromBase64String(hashedPassword);
            if ((src.Length != 0x31) || (src[0] != 0))
            {
                return false;
            }
            byte[] dst = new byte[0x10];
            Buffer.BlockCopy(src, 1, dst, 0, 0x10);
            byte[] buffer3 = new byte[0x20];
            Buffer.BlockCopy(src, 0x11, buffer3, 0, 0x20);
            using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(password, dst, 0x3e8))
            {
                buffer4 = bytes.GetBytes(0x20);
            }
            return AreHashesEqual(buffer3, buffer4);
        }

        private static bool AreHashesEqual(byte[] firstHash, byte[] secondHash)
        {
            int _minHashLength = firstHash.Length <= secondHash.Length ? firstHash.Length : secondHash.Length;
            var xor = firstHash.Length ^ secondHash.Length;
            for (int i = 0; i < _minHashLength; i++)
                xor |= firstHash[i] ^ secondHash[i];
            return 0 == xor;
        }
    }
}