using System.Collections.Generic;
using System.Threading.Tasks;
using GalaxyDto;

namespace GalaxyRepository.Contracts
{
    public interface IUserRepository
    {
        Task<UserDto> CreateAsync(UserDto user);
        Task<UserDto> GetAsync(int id);
        Task<bool> UpdateAsync(UserDto user);
        Task<bool> DeleteAsync(int id);

        Task<IEnumerable<UserDto>> GetUsersAsync(int pageNumber, int pageSize);
    }
}
