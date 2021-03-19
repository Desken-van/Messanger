using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Messanger.Domain.Interfaces;
using Messanger.Domain.DataBase;
using Messanger.Infrastructure.Data;

namespace Messanger.Controllers
{
    public class AdminController:Controller
    {
        IAccountRepository repoacc;
        public AdminController(IAccountRepository r)
        {
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
                Account user = await repoacc.CheckAccount(username);
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
                        return Ok();
                    }
                    else
                    {
                        return Ok("Already blocked");
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
                Account user = await repoacc.CheckAccount(username);
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
                        return Ok();
                    }
                    else
                    {
                        return Ok("Already Active");
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
                Account user = await repoacc.CheckAccount(username);
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
                        return Ok();
                    }
                    else
                    {
                        return Ok("Already Admin");
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
                Account user = await repoacc.CheckAccount(username);
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
                        return Ok();
                    }
                    else
                    {
                        return Ok("Already User");
                    }
                }

            }
        }
    }
}
