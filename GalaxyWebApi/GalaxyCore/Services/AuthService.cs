using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using GalaxyCore.Contracts;
using GalaxyRepository.Contracts;

namespace GalaxyCore.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IAuthRepository _authRepository;
        private readonly PasswordService _passwordService;

        public AuthService(IUserRepository userRepository, IAuthRepository authRepository, PasswordService passwordService)
        {
            _userRepository = userRepository;
            _authRepository = authRepository;
            _passwordService = passwordService;
        }


        public async Task<bool> AuthorizeAsync(string username, string password)
        {
            var user = await _userRepository.GetAsync(username);
            if (user == null)
                return false;

            var dbPasswordHash = await _authRepository.GetPasswordAsync(user.Id);

            return _passwordService.ComparePasswords(dbPasswordHash, password);
        }

        public async Task CreatePasswordAsync(string username, string password)
        {
            var user = await _userRepository.GetAsync(username);
            if (user is null)
                throw new Exception("CreatePasswordAsyncException");

            var hashedPasswordBytes = _passwordService.GetHashedPasswordBytes(password);

            await _authRepository.CreatePasswordAsync(user.Id, hashedPasswordBytes);
        }

        public async Task ChangePasswordAsync(string username, string newPassword)
        {
            var user = await _userRepository.GetAsync(username);

            if(user is null)
                throw new Exception("ChangePasswordAsyncException");

            var hashedPasswordBytes = _passwordService.GetHashedPasswordBytes(newPassword);

            await _authRepository.CreatePasswordAsync(user.Id, hashedPasswordBytes);

        }
    }
}