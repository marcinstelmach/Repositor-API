using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace RepositoryApp.API.Controllers
{
    [Route("api/Users")]
    public class UserController : Controller
    {
        [HttpGet]
        public IActionResult Test()
        {
            return Ok(new {Tekst = "Hej"});
        }
    }
}