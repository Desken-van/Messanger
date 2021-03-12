using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Messanger.Models;
using Messanger.DataBase;
using System.Linq;
using System.Collections.Generic;
using System;

namespace Messanger.Controllers
{
    public class SmsController : Controller
    {
        UsersContext db;
        public SmsController(UsersContext context)
        {
            db = context;
        }
        [Authorize]
        [HttpPost("/sms")]
        public IActionResult Sms(string username, string text, string recepient)
        {
            Account user = db.Logins.FirstOrDefault(x => x.Login == username);
            if (user == null)
            {
                return BadRequest();
            }
            Account Recepient = db.Logins.FirstOrDefault(x => x.Login == recepient);
            SMS sms = new SMS
            {
                Sender = user.Id,
                Sms = text,
                Recipient = Recepient.Id,
                Number = 1
            };
            if (sms == null || user == null || Recepient == null)
            {
                return BadRequest();
            }
            db.Sms.Add(sms);
            db.SaveChanges();
            var response = Ok();
            return Json(response);
        }

        [HttpPost("/Userlist")]
        public IActionResult UserList()
        {
            Account[] bigdata = db.Logins.ToArray();
            List<string> datausers = new List<string>();
            for (int i = 0; i < bigdata.Length; i++)
            {
                if (bigdata[i].Status == "Blocked")
                {
                    datausers.Add(Convert.ToString(bigdata[i].Login) + "*");
                }
                else datausers.Add(Convert.ToString(bigdata[i].Login));
            }
            var response = new
            {
                datausers
            };
            return Json(response);
        }

        [HttpPost("/Smslist")]
        public IActionResult SmsList(string username, string recepient)
        {
            Account user = db.Logins.FirstOrDefault(x => x.Login == username);
            Account recepienter = db.Logins.FirstOrDefault(x => x.Login == recepient);
            if (user == null || recepient == null)
            {
                return BadRequest();
            }
            SMS[] bigdata = db.Sms.ToArray();
            List<string> datasms = new List<string>();
            for (int i = 0; i < bigdata.Length; i++)
            {
                if (user.Id == bigdata[i].Sender && recepienter.Id == bigdata[i].Recipient)
                {
                    datasms.Add(Convert.ToString(bigdata[i].Sms));
                }
            }
            var response = new
            {
                datasms
            };
            return Json(response);
        }
  
        [HttpPost("/block")]
        public IActionResult Block(string admin, string username)
        {
            if (admin == null || username == null)
            {
                return BadRequest();
            }
            else
            {
                Account Admin = db.Logins.FirstOrDefault(x => x.Login == admin);
                if (Admin.Role == "Admin")
                {
                    Account user = db.Logins.FirstOrDefault(x => x.Login == username);
                    if (user.Status == "Active")
                    {
                        user.Status = "Blocked";
                        db.Logins.Update(user);
                        db.SaveChanges();
                        var response = Ok();
                        return Json(response);
                    }
                    else
                    {
                        var response = Ok();
                        return Json(response);
                    }
                }
                else return BadRequest();
            }
        }

        [HttpPost("/unblock")]
        public IActionResult UnBlock(string admin, string username)
        {
            if (admin == null || username == null)
            {
                return BadRequest();
            }
            else
            {
                Account Admin = db.Logins.FirstOrDefault(x => x.Login == admin);
                if (Admin.Role == "Admin")
                {
                    Account user = db.Logins.FirstOrDefault(x => x.Login == username);
                    if (user.Status == "Blocked")
                    {
                        user.Status = "Active";
                        db.Logins.Update(user);
                        db.SaveChanges();
                        var response = Ok();
                        return Json(response);
                    }
                    else
                    {
                        var response = Ok();
                        return Json(response);
                    }
                }
                else return BadRequest();
            }
        }

        [HttpPost("/putrole")]
        public IActionResult Putrole(string admin, string username)
        {
            if (admin == null || username == null)
            {
                return BadRequest();
            }
            else
            {
                Account Admin = db.Logins.FirstOrDefault(x => x.Login == admin);
                if (Admin.Role == "Admin")
                {
                    Account user = db.Logins.FirstOrDefault(x => x.Login == username);
                    if (user.Role == "User")
                    {
                        user.Role = "Admin";
                        db.Logins.Update(user);
                        db.SaveChanges();
                        var response = Ok();
                        return Json(response);
                    }
                    else
                    {
                        var response = Ok();
                        return Json(response);
                    }
                }
                else return BadRequest();
            }
        }
    }
}