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
        IMessageRepository reposms;
        public ListController(UsersContext context, IAccountRepository r,IMessageRepository m)
        {
            db = context;
            repoacc = r;
            reposms = m;
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
        public async Task<IActionResult> SmsList(string username, string recepient)
        {
            if (username == null || recepient == null)
            {
                return BadRequest();
            }
            AccountEntity user = await repoacc.CheckAccount(username);
            AccountEntity recepienter = await repoacc.CheckAccount(recepient);
            if (user == null || recepienter == null)
            {
                return BadRequest();
            }

            IQueryable<MessageEntity> bigdata = await reposms.GetMessageList() ;
            IQueryable<MessageEntity> datasms = from p in bigdata
                                                where (p.Sender == user.Login && p.Recipient == recepienter.Login) || (p.Recipient == user.Login && p.Sender == recepienter.Login)
                                                orderby p.Time
                                                select p;
            IQueryable<string> sms = from p in bigdata
                                                where (p.Sender == user.Login && p.Recipient == recepienter.Login) || (p.Recipient == user.Login && p.Sender == recepienter.Login)
                                                orderby p.Time
                                                select $"____________________________________\n||From:{p.Sender}|\n \n {p.Sms} \n \n|To:{p.Recipient}|  |{p.Time}||\n____________________________________\n";

            var response = new
            {
                datasms,
                sms
            };
            return Json(response);
        }   
    }
}
