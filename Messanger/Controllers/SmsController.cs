using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Messanger.Models;
using System.Linq;
using System.Collections.Generic;
using System;
using Messanger.Domain.Core;
using Messanger.Services.Interfaces;
using Messanger.Domain.Interfaces;
using Messanger.Infrastructure.Data;
using System.Threading.Tasks;

namespace Messanger.Controllers
{
    public class SmsController : Controller
    {
        UsersContext db;
        IMessageRepository reposms;
        IAddSms addsms;
        IAccountRepository repoacc;
        public SmsController(UsersContext context, IMessageRepository r, IAddSms a,IAccountRepository ar)
        {
            db = context;
            reposms = r;
            addsms = a;
            repoacc = ar;
        }
        [Authorize]
        [HttpPost("/sms")]
        public async Task<IActionResult> Sms(string username, string text, string recepient)
        {
            AccountEntity user = await repoacc.CheckAccount(username);
            if (user == null)
            {
                return BadRequest();
            }
            AccountEntity recepienter =await  repoacc.CheckAccount(recepient);
            if (recepienter == null || recepienter.Status == "Blocked")
            {
                return BadRequest();
            }
            else
            {
                MessageEntity sms = addsms.AddSms(user, text, recepienter);
                await reposms.Create(sms);
                var response = Ok();
                return Json(response);
            }
        }             
    }
}