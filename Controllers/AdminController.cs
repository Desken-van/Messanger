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
    public class AdminController:Controller
    {
        UsersContext db;
        public AdminController(UsersContext context)
        {
            db = context;
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("/block")]
        public IActionResult Block(string admin, string username)
        {
            if (admin == null || username == null)
            {
                return BadRequest();
            }
            else
            {
                Account Admin = db.Logins.FirstOrDefault(x => x.Login == admin);
                if (Admin.Role == "Admin")
                {
                    Account user = db.Logins.FirstOrDefault(x => x.Login == username);
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
                        var response = Ok();
                        return Json(response);
                    }
                }
                else return BadRequest();
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("/unblock")]
        public IActionResult UnBlock(string admin, string username)
        {
            if (admin == null || username == null)
            {
                return BadRequest();
            }
            else
            {
                Account Admin = db.Logins.FirstOrDefault(x => x.Login == admin);
                if (Admin.Role == "Admin")
                {
                    Account user = db.Logins.FirstOrDefault(x => x.Login == username);
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
                        var response = Ok();
                        return Json(response);
                    }
                }
                else return BadRequest();
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("/putrole")]
        public IActionResult Putrole(string admin, string username)
        {
            if (admin == null || username == null)
            {
                return BadRequest();
            }
            else
            {
                Account Admin = db.Logins.FirstOrDefault(x => x.Login == admin);
                if (Admin.Role == "Admin")
                {
                    Account user = db.Logins.FirstOrDefault(x => x.Login == username);
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
                        var response = Ok();
                        return Json(response);
                    }
                }
                else return BadRequest();
            }
        }
    }
}
