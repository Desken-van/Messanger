using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Messanger.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Authorization;

namespace Messanger.Controllers
{
    public class AccountController : Controller
    {
        UsersContext db;
        public AccountController(UsersContext context)
        {
            db = context;
        }
        [HttpPost("/token")]
        public IActionResult Token(string username, string password)
        {
            Account user = db.Logins.FirstOrDefault(x => x.Login == username);
            if (user == null)
            {
                return BadRequest();
            }
            bool confirm;
            confirm = VerifyHashedPassword(user.Password,password);
            if (confirm == true)
            {
                var identity = GetIdentity(username, user.Password);
                if (identity == null)
                {
                    return BadRequest(new { errorText = "Invalid username or password." });
                }

                var now = DateTime.UtcNow;
                // создаем JWT-токен
                var jwt = new JwtSecurityToken(
                        issuer: AuthOptions.ISSUER,
                        audience: AuthOptions.AUDIENCE,
                        notBefore: now,
                        claims: identity.Claims,
                        expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                        signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
                var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
                              
                Account[] bigdata = db.Logins.ToArray();
                List<string> datausers = new List<string>();
                for (int i = 0; i < bigdata.Length; i++)
                {                   
                datausers.Add(Convert.ToString(bigdata[i].Login));                   
                }

                var response = new
                {
                    access_token = encodedJwt,
                    username = identity.Name,
                    datausers
                };

                return Json(response);
            }
            else return null;
        }

        private ClaimsIdentity GetIdentity(string username, string password)
        {
            Account login = db.Logins.FirstOrDefault(x => x.Login == username && x.Password == password);
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

            // если пользователя не найдено
            return null;
        }
        [HttpPost("/register")]
        public IActionResult Register(string username, string password)
        {
            if (password.Length < 12 && password.Length > 4)
            {
                Account user = new Account();
                user.Login = username;
                user.Password = HashPassword(password);
                user.Role = "User";
                if (user == null)
                {
                    return BadRequest();
                }
                db.Logins.Add(user);
                db.SaveChanges();
                var response = Ok();
                return Json(response);               
            }
            else {
               return BadRequest(new { errorText = "Invalid username or password." });
            }
        }
        [HttpPost("/sms")]
        public IActionResult Sms(string username,string text , string recepient)
        {
            Account user = db.Logins.FirstOrDefault(x => x.Login == username);
            if (user == null)
            {
                return BadRequest();
            }
            if (user.Role == "Admin")
            {               
                var response = new
                {
                    
                };
                return Json(response);
            }
            else
            {
                Account admin = db.Logins.FirstOrDefault(x => x.Role == "Admin");
                SMS sms = new SMS
                {
                    Sender = user.Id,
                    Sms = text,
                    Recipient = admin.Id,
                    Number = 1
                };
                if (sms == null || user == null || admin == null)
                {
                    return BadRequest();
                }
                db.Sms.Add(sms);
                db.SaveChanges();
                var response = Ok();
                return Json(response);
            }
        }
        [HttpPost("/Userlist")]
        public IActionResult UserList()
        {
            Account[] bigdata = db.Logins.ToArray();
            List<string> datausers = new List<string>();
            for (int i = 0; i < bigdata.Length; i++)
            {
                datausers.Add(Convert.ToString(bigdata[i].Login));
            }
            var response = new
            {
                datausers
            };
            return Json(response);
        }
        [HttpPost ("/Smslist")]
        public IActionResult SmsList(string username,string recepient)
        {
            Account user = db.Logins.FirstOrDefault(x => x.Login == username);
            Account recepienter = db.Logins.FirstOrDefault(x => x.Login == recepient);
            if (user == null || recepient == null)
            {
                return BadRequest();
            }
            SMS[] bigdata = db.Sms.ToArray();
            List<string> data = new List<string>();
            for (int i = 0; i < bigdata.Length; i++)
            {
                if (user.Id == bigdata[i].Sender && recepienter.Id == bigdata[i].Recipient)
                {
                    data.Add(Convert.ToString(bigdata[i].Sms));
                }
            }
            var response = new
            {
                data
            };
            return Json(response);
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