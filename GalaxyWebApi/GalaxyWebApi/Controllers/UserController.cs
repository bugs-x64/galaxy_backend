using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaxyCore;
using GalaxyCore.Contracts;
using GalaxyCore.Services;
using GalaxyDto;
using GalaxyRepository.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GalaxyWebApi.Controllers
{
    /// <summary>
    /// Контроллер пользователя.
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IAuthService _authService;

        public UserController(IUserService userService, IJwtTokenService jwtTokenService, IAuthService authService)
        {
            _userService = userService;
            _jwtTokenService = jwtTokenService;
            _authService = authService;
        }

        private static object GetUserData(UserDto user) => new
        {
            user.Id, 
            user.Created, 
            user.Username, 
            user.FirstName, 
            user.Birthdate
        };

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> CreateAsync(NewUserDto authData)
        {
            var result = await _userService.CreateAsync(authData);

            await _authService.CreatePasswordAsync(authData.Username, authData.Password);

            var token = _jwtTokenService.GenerateToken(result.Username);
            return Ok(token);
        }

        [HttpGet]
        public async Task<IActionResult> GetUsersAsync(int page = 0, int size = 1000)
        {
            var users = await _userService.GetUsersAsync(page, size);
            var result = users.Select(GetUserData);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserAsync(int id)
        {
            var user = await _userService.GetAsync(id);
            var result = GetUserData(user);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUserAsync(int id, UserDto user)
        {
            if (id != user.Id)
                return BadRequest();

            try
            {
                await _userService.UpdateAsync(user);

                return Ok();
            }
            catch (CustomException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserAsync(int id)
        {
            try
            {
                return Ok(await _userService.DeleteAsync(id));
            }
            catch (CustomException e)
            {
                return BadRequest(e.Message);
            }
            
        }
    }
}
