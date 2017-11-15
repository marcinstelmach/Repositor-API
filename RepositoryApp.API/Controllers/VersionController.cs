using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace RepositoryApp.API.Controllers
{
    [Produces("application/json")]
    [Route("api/Version")]
    public class VersionController : Controller
    {
    }
}