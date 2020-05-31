using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
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

        public UserService(IAuthRepository authRepository, IUserRepository userRepository)
        {
            _authRepository = authRepository;
            _userRepository = userRepository;
        }

        public async Task<UserDto> CreateAsync(NewUserDto newUser)
        {
            var inputData = new UserDto()
            {
                Username = newUser.Username,
                Birthdate = newUser.Birthdate,
                FirstName = newUser.Firstname
            };

            var outputData = await _userRepository.CreateAsync(inputData);
            await _authRepository.CreatePassword(outputData.Id, Encoding.UTF8.GetBytes(newUser.Password));

            return outputData;
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