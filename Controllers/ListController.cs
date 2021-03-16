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
            IQueryable<Account> bigdata = db.Logins;
            IQueryable<string> users = from p in bigdata
                                         where p.Role == "User" && p.Status == "Active"
                                         select p.Login;
            IQueryable<string> admins = from p in bigdata
                        where p.Role == "Admin" && p.Status == "Active"
                        select "$" + p.Login;

            IQueryable<string> blocks = from p in bigdata
                        where p.Role == "User" && p.Status == "Blocked"
                        select p.Login + "*";

            var users_and_admins = users.Union(admins);
            var datausers = users_and_admins.Union(blocks);
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
            if (username == null || recepient == null)
            {
                return BadRequest();
            }
            Account user = db.Logins.FirstOrDefault(x => x.Login == username);
            Account recepienter = db.Logins.FirstOrDefault(x => x.Login == recepient);
            if (user == null || recepienter == null)
            {
                return BadRequest();
            }

            IQueryable<SMS> bigdata = db.Sms;
            IQueryable<string> datasms= from p in bigdata
                                         where (p.Sender == user.Id && p.Recipient == recepienter.Id) || (p.Recipient == user.Id && p.Sender == recepienter.Id)
                                         orderby p.Number                                 
                                         select p.Sender +":"+p.Sms;            
            
            var response = new
            {
                datasms
            };
            return Json(response);
        }      
    }
}
