using System;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using GalaxyCore.Contracts;
using GalaxyDto;
using GalaxyRepository.Contracts;
using GalaxyRepository.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.Swagger.Annotations;

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
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IUserRepository _userRepository;
        private readonly IAuthRepository _authRepository;

        public UserController(IJwtTokenService jwtTokenService, IUserRepository userRepository, IAuthRepository authRepository)
        {
            _jwtTokenService = jwtTokenService ?? throw new ArgumentNullException(nameof(jwtTokenService));
            _userRepository = userRepository;
            _authRepository = authRepository;
        }

        private object GetUserData(UserDto user) => new {user.Id, user.Created, user.Username, user.FirstName, user.Birthdate};

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Create(NewUserDto authData)
        {
            var userDto = new UserDto()
            {
                Username = authData.Username,
                Birthdate = authData.Birthdate,
                FirstName = authData.Firstname
            };
           var qwe =  await _userRepository.CreateAsync(userDto);

           await _authRepository.CreatePassword(qwe.Id, Encoding.UTF8.GetBytes(authData.Password));


            var token = _jwtTokenService.GenerateToken(authData.Username);
            return Ok(token);
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userRepository.GetUsersAsync(0, 1000);
            return Ok(users.Select(GetUserData));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _userRepository.GetAsync(id);
            return Ok(GetUserData(user));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUser(int id, UserDto user)
        {
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            return Ok(await _userRepository.DeleteAsync(id));
        }
    }
}
