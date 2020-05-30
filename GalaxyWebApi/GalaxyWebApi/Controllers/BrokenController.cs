using Microsoft.AspNetCore.Mvc;

namespace GalaxyWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BrokenController : ControllerBase
    {
        [HttpPost]
        public object Get()
        {
            return "";
        }
    }
}
