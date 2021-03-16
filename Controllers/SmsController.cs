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
            Account recepienter = db.Logins.FirstOrDefault(x => x.Login == recepient);
            if (recepienter == null || recepienter.Status == "Blocked")
            {
                return BadRequest();
            }
            else
            {
                SMS x = db.Sms.OrderByDescending(x => x.Number).FirstOrDefault();
                int count = 1;
                if (x.Number != 0)
                {
                    count = 1 + x.Number;
                }
                SMS sms = new SMS
                {
                    Sender = user.Id,
                    Sms = text,
                    Recipient = recepienter.Id,
                    Number = count
                };               
                db.Sms.Add(sms);
                db.SaveChanges();
                var response = Ok();
                return Json(response);
            }
        }             
    }
}