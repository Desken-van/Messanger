using Messanger.DataBase;
using Messanger.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Messanger.Controllers
{  
    public class ListController : Controller
    {
        UsersContext db;
        public ListController(UsersContext context)
        {
            db = context;
        }
        [Authorize]
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
                else if(bigdata[i].Role == "Admin")
                {
                    datausers.Add(Convert.ToString("$"+ bigdata[i].Login));
                }
                else datausers.Add(Convert.ToString(bigdata[i].Login));
            }
            var response = new
            {
                datausers
            };
            return Json(response);
        }
        [Authorize]
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
                else if (user.Id == bigdata[i].Recipient && recepienter.Id == bigdata[i].Sender)
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
    }
}
