using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.Swagger.Annotations;
using WebService1.BLL.Contracts;

namespace GalaxyWebApi.Controllers
{
    /// <summary>
    /// Контроллер авторизации.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    public class AuthController : ControllerBase
    {
        private readonly IJwtTokenService _jwtTokenService;

        public AuthController(IJwtTokenService jwtTokenService)
        {
            _jwtTokenService = jwtTokenService ?? throw new ArgumentNullException(nameof(jwtTokenService));
        }

        /// <summary>
        /// Генерирует токен авторизации по указанному логину/паролю.
        /// </summary>       
        /// <returns>Сгенерированный токен.</returns>        
        [AllowAnonymous]
        [HttpGet("token")]
        [SwaggerResponse((int)HttpStatusCode.OK, Description = "Generated token")]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, Description = "Unexpected error")]
        public IActionResult GenerateToken()
        {
            var token = _jwtTokenService.GenerateToken();
            return Ok(token);
        }

        /// <summary>
        /// Validate sample token
        /// </summary>
        /// <param name="token">Token for validation</param>
        /// <returns>Token validation status</returns>        
        [HttpPost("validate")]
        [SwaggerResponse((int)HttpStatusCode.OK, Description = "Token validation status")]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, Description = "Bad request for missing or invalid parameter")]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, Description = "Unexpected error")]
        public IActionResult ValidateToken([FromBody] string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return BadRequest();
            }
            var isValid = _jwtTokenService.ValidateToken(token);

            return Ok(new { isValid });
        }
    }

}
