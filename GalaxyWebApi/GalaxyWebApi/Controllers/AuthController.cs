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
        /// <response code="200">Успешная авторизация</response>
        /// <response code="401">Неверный логин/пароль</response>      
        [AllowAnonymous]
        [HttpPost]
        [SwaggerResponse((int)HttpStatusCode.OK)]
        [SwaggerResponse((int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> GetTokenAsync(AuthorizationDto authData)
        {
            var isAuthorized = await _authService.AuthorizeAsync(authData.Username, authData.Password);
            if (!isAuthorized)
                return Unauthorized();

            var token = _jwtTokenService.GenerateToken(authData.Username);

            return Ok(token);
        }

        /// <response code="200">Валидный токен</response>
        /// <response code="400">Невалидный токен</response>
        [AllowAnonymous]
        [HttpGet]
        [SwaggerResponse((int)HttpStatusCode.OK)]
        [SwaggerResponse((int)HttpStatusCode.BadRequest)]
        public IActionResult ValidateToken(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return BadRequest();
            }
            var isValid = _jwtTokenService.ValidateToken(token);

            return isValid ? (IActionResult)Ok() : BadRequest();
        }

        /// <response code="200">Пароль успешно изменен</response>
        /// <response code="400">Ошибка в теле запроса</response>
        [HttpPost]
        [SwaggerResponse((int)HttpStatusCode.OK)]
        [SwaggerResponse((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> ChangePasswordAsync(ChangePasswordDto changePasswordData)
        {
            var isValidRequest = string.IsNullOrEmpty(changePasswordData.Username)
                                || string.IsNullOrEmpty(changePasswordData.NewPassword)
                                || string.IsNullOrEmpty(changePasswordData.OldPassword);
            if (isValidRequest)
                return BadRequest();

            var identityName = User.Identity.Name ?? string.Empty;
            var isIdentityNameNotEqualsRequested = !identityName.Equals(changePasswordData.Username, StringComparison.InvariantCultureIgnoreCase);
            if (isIdentityNameNotEqualsRequested)
                return BadRequest();

            var passwordsEquals = changePasswordData.NewPassword.Equals(changePasswordData.OldPassword, StringComparison.InvariantCultureIgnoreCase);
            if (passwordsEquals)
                return BadRequest("Old and new password are equals");

            await _authService.ChangePasswordAsync(changePasswordData.Username, changePasswordData.NewPassword);

            return Ok();
        }
    }
}
