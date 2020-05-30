using System.Collections.Generic;
using System.Threading.Tasks;
using GalaxyDto;
using GalaxyRepository.Models;

namespace GalaxyRepository.Contracts
{
    public interface IAuthRepository
    {
        Task CreatePassword(int userId, byte[] passwordHashBytes);

        Task<byte[]> GetPassword(int userId);
    }

    public class AuthRepository : IAuthRepository
    {
        private GalaxyContext _context;

        public AuthRepository(GalaxyContext context)
        {
            _context = context;
        }

        public async Task CreatePassword(int userId, byte[] passwordHashBytes)
        {
            await _context.Password.AddAsync(new Password()
            {
                PasswordHash = passwordHashBytes,
                Userid = userId
            });

            await _context.SaveChangesAsync(true);
        }

        public Task<byte[]> GetPassword(int userId)
        {
            throw new System.NotImplementedException();
        }
    }
}
