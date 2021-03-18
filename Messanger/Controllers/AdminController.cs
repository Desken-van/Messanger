using Messanger.Domain.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Messanger.Infrastructure.Data;
using Messanger.Domain.Interfaces;

namespace Messanger.Controllers
{
    public class AdminController:Controller
    {
        UsersContext db;
        IAccountRepository repoacc;
        public AdminController(UsersContext context ,IAccountRepository r)
        {
            db = context;
            repoacc = r; 
        }
        
        [Authorize(Roles = "Admin")]
        [HttpPost("/block")]
        public async Task<IActionResult> Block(string username)
        {
            if (username == null)
            {
                return BadRequest();
            }           
            else
            {
                AccountEntity user = await repoacc.CheckAccount(username);
                if (user == null)
                {
                    return BadRequest();
                }
                else
                {
                    if (user.Status == "Active")
                    {
                        user.Status = "Blocked";
                        await repoacc.Update(user);
                        var response = Ok();
                        return Json(response);
                    }
                    else
                    {
                        var response = Ok("Already blocked");
                        return Json(response);
                    }
                }
                
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("/unblock")]
        public async Task<IActionResult> UnBlock(string username)
        {
            if (username == null)
            {
                return BadRequest();
            }
            else
            {
                AccountEntity user = await repoacc.CheckAccount(username);
                if (user == null)
                {
                    return BadRequest();
                }
                else
                {
                    if (user.Status == "Blocked")
                    {
                        user.Status = "Active";
                        await repoacc.Update(user);
                        var response = Ok();
                        return Json(response);
                    }
                    else
                    {
                        var response = Ok("Already Active");
                        return Json(response);
                    }
                }

            }
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("/putrole")]
        public async Task<IActionResult> Putrole(string username)
        {
            if (username == null)
            {
                return BadRequest();
            }
            else
            {
                AccountEntity user = await repoacc.CheckAccount(username);
                if (user == null)
                {
                    return BadRequest();
                }
                else
                {
                    if (user.Role == "User")
                    {
                        user.Role = "Admin";
                        await repoacc.Update(user);
                        var response = Ok();
                        return Json(response);
                    }
                    else
                    {
                        var response = Ok("Already Admin");
                        return Json(response);
                    }
                }

            }
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("/unrole")]
        public async Task<IActionResult> Unrole(string username)
        {
            if (username == null)
            {
                return BadRequest();
            }
            else
            {
                AccountEntity user = await repoacc.CheckAccount(username);
                if (user == null)
                {
                    return BadRequest();
                }
                else
                {
                    if (user.Role == "Admin")
                    {
                        user.Role = "User";
                        await repoacc.Update(user);
                        var response = Ok();
                        return Json(response);
                    }
                    else
                    {
                        var response = Ok("Already User");
                        return Json(response);
                    }
                }

            }
        }
    }
}
