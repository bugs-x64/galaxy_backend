using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GalaxyCore.Contracts;
using GalaxyDto;
using GalaxyRepository.Contracts;

namespace GalaxyCore.Services
{
    /// <summary>
    /// Сервис управления учетными записями пользователей.
    /// </summary>
    public class UserService : IUserService
    {
        private readonly IAuthRepository _authRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(IAuthRepository authRepository, IUserRepository userRepository, IMapper mapper)
        {
            _authRepository = authRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<UserDto> CreateAsync(NewUserDto newUser)
        {
            var inputData = _mapper.Map<UserDto>(newUser);

            var user = await _userRepository.GetAsync(newUser.Username);

            if(user != null)
                throw new ArgumentException($"Пользователь {newUser.Username} уже существует.");

            return await _userRepository.CreateAsync(inputData);
        }

        public Task<UserDto> GetAsync(int id)
        {
            return _userRepository.GetAsync(id);
        }

        public Task<bool> UpdateAsync(UserDto user)
        {
            return _userRepository.UpdateAsync(user);
        }

        public Task<bool> DeleteAsync(int id)
        {
            return _userRepository.DeleteAsync(id);
        }

        public Task<IEnumerable<UserDto>> GetUsersAsync(int pageNumber, int pageSize)
        {
            return _userRepository.GetUsersAsync(pageNumber,pageSize);
        }
    }
}