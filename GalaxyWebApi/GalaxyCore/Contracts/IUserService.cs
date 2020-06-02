using System.Collections.Generic;
using System.Threading.Tasks;
using GalaxyDto;

namespace GalaxyCore.Contracts
{
    /// <summary>
    /// Интерфейс сервиса управления учетными записями пользователей.
    /// </summary>
    public interface IUserService
    {
        Task<UserDto> CreateAsync(NewUserDto newUser);
        Task<UserDto> GetAsync(string username);
        Task<bool> UpdateAsync(UserDto user);
        Task<bool> DeleteAsync(string username);
        Task<IEnumerable<UserDto>> GetUsersAsync(int pageNumber, int pageSize);
    }
}