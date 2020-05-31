using System;
using GalaxyCore;
using Microsoft.AspNetCore.Mvc;

namespace GalaxyWebApi.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class BrokenController : ControllerBase
    {
        [HttpGet]
        public void UnhandledException()
        {
            throw new NullReferenceException();
        }

        [HttpGet]
        public void LogicException()
        {
            throw new CustomException();
        }
    }
}
