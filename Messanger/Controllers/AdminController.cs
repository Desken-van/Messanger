using Messanger.Domain.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Messanger.Infrastructure.Data;

namespace Messanger.Controllers
{
    public class AdminController:Controller
    {
        UsersContext db;
        public AdminController(UsersContext context)
        {
            db = context;
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("/block")]
        public IActionResult Block(string username)
        {
            if (username == null)
            {
                return BadRequest();
            }           
            else
            {
                AccountEntity user = db.Logins.FirstOrDefault(x => x.Login == username);
                if (user == null)
                {
                    return BadRequest();
                }
                else
                {
                    if (user.Status == "Active")
                    {
                        user.Status = "Blocked";
                        db.Logins.Update(user);
                        db.SaveChanges();
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
        public IActionResult UnBlock(string username)
        {
            if (username == null)
            {
                return BadRequest();
            }
            else
            {
                AccountEntity user = db.Logins.FirstOrDefault(x => x.Login == username);
                if (user == null)
                {
                    return BadRequest();
                }
                else
                {
                    if (user.Status == "Blocked")
                    {
                        user.Status = "Active";
                        db.Logins.Update(user);
                        db.SaveChanges();
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
        public IActionResult Putrole(string username)
        {
            if (username == null)
            {
                return BadRequest();
            }
            else
            {
                AccountEntity user = db.Logins.FirstOrDefault(x => x.Login == username);
                if (user == null)
                {
                    return BadRequest();
                }
                else
                {
                    if (user.Role == "User")
                    {
                        user.Role = "Admin";
                        db.Logins.Update(user);
                        db.SaveChanges();
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
        public IActionResult Unrole(string username)
        {
            if (username == null)
            {
                return BadRequest();
            }
            else
            {
                AccountEntity user = db.Logins.FirstOrDefault(x => x.Login == username);
                if (user == null)
                {
                    return BadRequest();
                }
                else
                {
                    if (user.Role == "Admin")
                    {
                        user.Role = "User";
                        db.Logins.Update(user);
                        db.SaveChanges();
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
