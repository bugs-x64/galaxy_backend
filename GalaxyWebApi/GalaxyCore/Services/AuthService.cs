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
        //todo добавить соль к паролю

        private readonly IUserRepository _userRepository;
        private readonly IAuthRepository _authRepository;
        private static readonly Encoding Encoding = Encoding.UTF8;

        public AuthService(IUserRepository userRepository, IAuthRepository authRepository)
        {
            _userRepository = userRepository;
            _authRepository = authRepository;
        }

        private static byte[] GetHashedPasswordBytes(string password)
        {
            using var sha256Hash = SHA256.Create();
            
            return sha256Hash.ComputeHash(Encoding.GetBytes(password));
        }

        public async Task<bool> AuthorizeAsync(string username, string password)
        {
            var user = await _userRepository.GetAsync(username);

            if (user == null)
                return false;

            var dbPasswordHash = await _authRepository.GetPasswordAsync(user.Id);

            var currentPasswordHash = GetHashedPasswordBytes(password);

            var isPasswordsEquals = dbPasswordHash.SequenceEqual(currentPasswordHash);

            return isPasswordsEquals;
        }

        public async Task CreatePasswordAsync(string username, string password)
        {
            var user = await _userRepository.GetAsync(username);

            var hashedPasswordBytes = GetHashedPasswordBytes(password);

            await _authRepository.CreatePasswordAsync(user.Id,hashedPasswordBytes);
        }

        public async Task ChangePasswordAsync(string username, string newPassword)
        {
            var user = await _userRepository.GetAsync(username);

            await _authRepository.CreatePasswordAsync(user.Id, GetHashedPasswordBytes(newPassword));

        }
    }
}