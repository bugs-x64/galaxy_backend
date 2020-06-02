using System.Collections.Generic;
using System.Threading.Tasks;
using GalaxyDto;
using Microsoft.EntityFrameworkCore.Internal;

namespace GalaxyRepository.Contracts
{
    public interface IAuthRepository
    {
        Task CreatePasswordAsync(int userId, byte[] passwordHashBytes);

        Task<byte[]> GetPasswordAsync(int userId);
    }
}
