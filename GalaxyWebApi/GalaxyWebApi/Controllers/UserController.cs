﻿using System;
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
            user.Birthdate,
            user.Amount
        };

        /// <summary>
        /// Создать пользователя.
        /// </summary>
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> CreateAsync(NewUserDto authData)
        {
            var user = await _userService.GetAsync(authData.Username);
            if (user != null)
                return BadRequest($"{authData.Username} был создан ранее.");

            var result = await _userService.CreateAsync(authData);

            await _authService.CreatePasswordAsync(authData.Username, authData.Password);

            var token = _jwtTokenService.GenerateToken(result.Username);
            return Ok(token);
        }

        /// <summary>
        /// Получить информацию о пользователях.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetUsersAsync(int page = 0, int size = 1000)
        {
            var users = await _userService.GetUsersAsync(page, size);
            var result = users.Select(GetUserData);
            return Ok(result);
        }

        /// <summary>
        /// Получить информацию о пользователе.
        /// </summary>
        [HttpGet("{username}")]
        public async Task<IActionResult> GetUserAsync(string username)
        {
            var user = await _userService.GetAsync(username);
            if (user is null)
                return NotFound();

            var result = GetUserData(user);
            return Ok(result);
        }

        /// <summary>
        /// Обновить данные пользователя.
        /// </summary>
        [HttpPut("{username}")]
        public async Task<IActionResult> UpdateUserAsync(string username, UserDto user)
        {
            if (username != user.Username)
                return BadRequest();

            //сюда можно добавить проверку через User.claims, текущий пользователь или администратор

            var isUpdated = await _userService.UpdateAsync(user);
            return isUpdated ? (IActionResult) Ok() : BadRequest();
        }

        /// <summary>
        /// Удалить пользователя.
        /// </summary>
        [HttpDelete("{username}")]
        public async Task<IActionResult> DeleteUserAsync(string username)
        {
            var isDeleted = await _userService.DeleteAsync(username);

            return isDeleted ? (IActionResult) Ok() : BadRequest();
        }
    }
}
