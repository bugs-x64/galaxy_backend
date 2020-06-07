using System;
using GalaxyCore;
using Microsoft.AspNetCore.Mvc;

namespace GalaxyWebApi.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    [Produces("application/json")]
    public class BrokenController : ControllerBase
    {
        [HttpGet]
        public void UnhandledException()
        {
            throw new NullReferenceException();
        }
    }
}
