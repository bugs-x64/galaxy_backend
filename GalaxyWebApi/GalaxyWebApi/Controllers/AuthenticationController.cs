using System;
using System.Net;
using GalaxyCore.Contracts;
using GalaxyDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.Swagger.Annotations;

namespace GalaxyWebApi.Controllers
{
    /// <summary>
    /// Контроллер авторизации.
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("[controller]/[action]")]
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
        [HttpPost]
        [SwaggerResponse((int)HttpStatusCode.OK, Description = "Токен сгенерирован")]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, Description = "Неверный логин/пароль")]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, Description = "Внутренняя ошибка сервера")]
        public IActionResult GenerateToken(AuthorizationDto authData)
        {
            var token = _jwtTokenService.GenerateToken(authData.Username);

            return Ok(token);
        }
 
        [HttpPost]
        [SwaggerResponse((int)HttpStatusCode.OK, Description = "Токен в валидном статусе")]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, Description = "Ошибка в теле запроса")]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, Description = "Внутренняя ошибка сервера")]
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
