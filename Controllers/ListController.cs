using Messanger.Domain.DataBase;
using Messanger.Domain.Interfaces;
using Messanger.Infrastructure.Data;
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
        IAccountRepository repoacc;
        IMessageRepository reposms;
        public ListController(IAccountRepository r,IMessageRepository m)
        {
            repoacc = r;
            reposms = m;
        }
        [Authorize]
        [HttpPost("/Userlist")]
        public async Task<IActionResult> UserList()
        {
            List<string> datausers = await repoacc.GetAccountSite();
            var response = new
            {
                datausers
            };
            return Json(response);
        }
        [Authorize]
        [HttpPost("/Smslist")]
        public async Task<IActionResult> SmsList(string recepient)
        {
            if (recepient == null)
            {
                return BadRequest();
            }
            Account recepienter = await repoacc.CheckAccount(recepient);
            if (recepienter == null)
            {
                return BadRequest();
            }
            List<Message> datasms = await reposms.GetMessageList(User.Identity.Name,recepient);
            List<string> sms = await reposms.GetMessageSite(User.Identity.Name, recepient);
            var response = new
            {
                datasms,
                sms
            };
            return Json(response);
        }   
    }
}
