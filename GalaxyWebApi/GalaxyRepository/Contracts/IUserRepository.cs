using System.Collections.Generic;
using System.Threading.Tasks;
using GalaxyDto;

namespace GalaxyRepository.Contracts
{
    public interface IUserRepository
    {
        Task<UserDto> CreateAsync(UserDto user);
        Task<UserDto> GetAsync(string username);
        Task<bool> UpdateAsync(UserDto user);
        Task<bool> DeleteAsync(string username);

        Task<IEnumerable<UserDto>> GetUsersAsync(int pageNumber, int pageSize);
    }
}
