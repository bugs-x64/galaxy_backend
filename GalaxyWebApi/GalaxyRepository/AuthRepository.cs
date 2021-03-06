using System.Linq;
using System.Threading.Tasks;
using GalaxyRepository.Contracts;
using GalaxyRepository.Models;
using Microsoft.EntityFrameworkCore;

namespace GalaxyRepository
{
    public class AuthRepository : IAuthRepository
    {
        private readonly GalaxyContext _context;

        public AuthRepository(GalaxyContext context)
        {
            _context = context;
        }

        public async Task CreatePasswordAsync(int userId, byte[] passwordHashBytes)
        {
            await _context.Password.AddAsync(new Password()
            {
                PasswordHash = passwordHashBytes,
                Userid = userId
            });

            await _context.SaveChangesAsync(true);
        }

        public async Task<byte[]> GetPasswordAsync(int userId)
        {
            var lastPasswordId =  _context.Password.Max(x=>x.Id);

            var password = await _context.Password.SingleAsync(x => x.Id == lastPasswordId);

            return password.PasswordHash;
        }
    }
}