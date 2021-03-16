using Messanger.Domain.Core;
using Messanger.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Messanger.Infrastructure.Data;

namespace Messanger.Controllers
{  
    public class ListController : Controller
    {
        UsersContext db;
        IAccountRepository repoacc;
        public ListController(UsersContext context, IAccountRepository r)
        {
            db = context;
            repoacc = r;      
        }
        [Authorize]
        [HttpPost("/Userlist")]
        public IActionResult UserList()
        {
            IQueryable<AccountEntity> bigdata = db.Logins;
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
            AccountEntity user = repoacc.CheckAccount(username);
            AccountEntity recepienter = repoacc.CheckAccount(recepient);
            if (user == null || recepienter == null)
            {
                return BadRequest();
            }

            IQueryable<MessageEntity> bigdata = db.Sms;
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
