using Messanger.Domain.DataBase;
using Messanger.Domain.Interfaces;
using Messanger.Infrastructure.Data;
using Messanger.Models.InterfaceModels;
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
            IEnumerable<Account> list = await repoacc.GetAccountList();
            var listusers = list.Select(x => new AccountResponse {
                Login = x.Login,
                Role = x.Role,
                Status = x.Status
            });
            var datausers = await repoacc.GetAccountSite();
            var response = new
            {
                datausers,
                listusers
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
            IEnumerable<Message> datasms = await reposms.GetMessageList(User.Identity.Name,recepient);
            var text= datasms.Select(x => new MessageResponse { 
            Sender = x.Sender,
            Sms = x.Sms,
            Recipient = x.Recipient,
            Time = x.Time
            });
            var sms = await reposms.GetMessageSite(User.Identity.Name, recepient);
            var response = new
            {
                text,
                sms
            };
            return Json(response);
        }   
    }
}
