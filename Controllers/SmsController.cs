using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Messanger.Models;
using System.Linq;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using Messanger.Domain.Interfaces;
using Messanger.Domain.DataBase;
using Messanger.Infrastructure.Data;

namespace Messanger.Controllers
{
    public class SmsController : Controller
    {
        IMessageRepository reposms;
        IAccountRepository repoacc;
        public SmsController(IMessageRepository r,IAccountRepository ar)
        {
            reposms = r;
            repoacc = ar;
        }
        [Authorize]
        [HttpPost("/sms")]
        public async Task<IActionResult> Sms(string text, string recepient)
        {           
            Account recepienter =await  repoacc.CheckAccount(recepient);
            if (recepienter == null || recepienter.Status == "Blocked")
            {
                return BadRequest();
            }
            else
            {
                await reposms.Add(User.Identity.Name, text, recepient);
                return Ok();
            }
        }             
    }
}