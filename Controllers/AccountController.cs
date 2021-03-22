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
using Messanger.Models.InterfaceModels;
using Messanger.Domain.Interfaces;
using System.Threading.Tasks;
using Messanger.Domain.DataBase;
using Messanger.Infrastructure.Data;
using Messanger.Domain.Repository;

namespace Messanger.Controllers
{
    public class AccountController : Controller
    {
        IAccountRepository repoacc;
        public AccountController(IConfiguration configuration, IAccountRepository r)
        {
            Configuration = configuration;
            repoacc = r;
        }
        public IConfiguration Configuration { get; }
        public Models.ProgramModels.AuthorizationOptions authoption { get; private set; }
           
        [HttpPost("/token")]
        public async Task<IActionResult> Token(string username, string password)
        {        
            Account user = await repoacc.CheckAccount(username);
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
                Account account = await repoacc.GetAccount(username,user.Password);
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
        public async Task<IActionResult> Register(string username, string password)
        {
            if ((password.Length > 12 && password.Length < 4)||username == null)
            {
                return BadRequest(new { errorText = "Invalid username or password." });
            }
            else {              
                IEnumerable<Account> bigdata = await repoacc.GetAccountList();
                foreach (Account s in bigdata)
                {
                    if (username == s.Login)
                    {
                        return BadRequest();
                    }
                }
                await repoacc.Add(username, HashPassword(password));
                return Ok();
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