﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Messanger.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        [Authorize]
        [Route("getlogin")]
        [HttpGet]
        public IActionResult GetLogin()
        {
            return Ok($"Ваш логин: {User.Identity.Name}");
        }

        [Authorize(Roles = "Admin")]
        [Route("getrole")]
        [HttpGet]
        public IActionResult GetRole()
        {
            return Ok("Ваша роль: администратор");
        }       
    }
}