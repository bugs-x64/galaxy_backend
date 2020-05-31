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
        Task<UserDto> GetAsync(int id);
        Task<bool> UpdateAsync(UserDto user);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<UserDto>> GetUsersAsync(int pageNumber, int pageSize);
    }
}