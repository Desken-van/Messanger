using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Messanger.Models;
using System.Linq;

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

        [Authorize]
        [Route("getrole")]
        [HttpGet]
        public IActionResult GetRole()
        {
            if (User.IsInRole("Admin") == true) 
            {
                return Ok("Ваша роль: администратор");
            }
            else
            {
                return Ok("Ваша роль: пользователь");
            }            
        }                     
    }
}