using System;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
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
        private readonly IAuthService _authService;

        public AuthController(IJwtTokenService jwtTokenService, IAuthService authService)
        {
            _jwtTokenService = jwtTokenService ?? throw new ArgumentNullException(nameof(jwtTokenService));
            _authService = authService;
        }

        /// <summary>
        /// Генерирует токен авторизации по указанному логину/паролю.
        /// </summary>       
        /// <returns>Сгенерированный токен.</returns>        
        [AllowAnonymous]
        [HttpPost]
        [SwaggerResponse((int)HttpStatusCode.OK, Description = "Токен сгенерирован")]
        [SwaggerResponse((int)HttpStatusCode.Unauthorized, Description = "Неверный логин/пароль")]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, Description = "Внутренняя ошибка сервера")]
        public async Task<IActionResult> GenerateTokenAsync(AuthorizationDto authData)
        {
            var isAuthorized = await _authService.AuthorizeAsync(authData.Username, authData.Password);
            if (!isAuthorized)
                return Unauthorized();

            var token = _jwtTokenService.GenerateToken(authData.Username);

            return Ok(token);
        }

        [HttpGet]
        [SwaggerResponse((int)HttpStatusCode.OK, Description = "Токен в валидном статусе")]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, Description = "Ошибка в теле запроса")]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, Description = "Внутренняя ошибка сервера")]
        public IActionResult ValidateToken(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return BadRequest();
            }
            var isValid = _jwtTokenService.ValidateToken(token);

            return Ok(new { isValid });
        }

        [HttpPost]
        [SwaggerResponse((int)HttpStatusCode.OK, Description = "Пароль успешно изменен")]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, Description = "Ошибка в теле запроса")]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, Description = "Внутренняя ошибка сервера")]
        public async Task<IActionResult> ChangePasswordAsync(ChangePasswordDto changePasswordData)
        {
            var isValidRequest = string.IsNullOrEmpty(changePasswordData.Username)
                                || string.IsNullOrEmpty(changePasswordData.NewPassword)
                                || string.IsNullOrEmpty(changePasswordData.OldPassword);
            if (isValidRequest)
                return BadRequest();

            var identityName = User.Identity.Name ?? string.Empty;
            if (identityName.Equals(changePasswordData.Username, StringComparison.InvariantCultureIgnoreCase))
                return Unauthorized();

            var passwordsEquals = changePasswordData.NewPassword.Equals(changePasswordData.OldPassword, StringComparison.InvariantCultureIgnoreCase);
            if (passwordsEquals)
                return BadRequest("Old and new password are equals");

            await _authService.ChangePasswordAsync(changePasswordData.Username,changePasswordData.NewPassword);

            return Ok();
        }
    }
}
